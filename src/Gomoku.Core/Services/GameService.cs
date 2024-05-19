using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Dtos.SignalR;
using Gomoku.Core.Exceptions;
using Gomoku.Core.Hubs;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper, IWaitingListRepository waitingListRepository, IHubContext<GameHub> gameHub) : IGameService
{
    public async Task<GameCreatedDto> Create()
    {
        var players = await waitingListRepository.GetTop2Async();

        if (players == null || players.Count < 2)
            return null;

        var gameCode = Guid.NewGuid();

        var randomNumber = new Random().Next(0, 2);
        var game = new Game
        {
            BlackName = players[randomNumber].PlayerName,
            WhiteName = players[randomNumber ^ 1].PlayerName,
            Code = gameCode,
            State = GameState.Created,
            Time = 60,
            BlackTime = 60,
            WhiteTime = 60,
        };

        await repository.AddAsync(game);
        await waitingListRepository.DeleteManyAsync(x => x.PlayerName == players[0].PlayerName || x.PlayerName == players[1].PlayerName);

        await gameHub.Clients.Client(players[0].ConnectionId).SendAsync("GameCreated", game.Code);
        await gameHub.Clients.Client(players[1].ConnectionId).SendAsync("GameCreated", game.Code); // how to handle observers, add them to gorup? is there any limit in groups?

        // todo: disconnect clients?

        return mapper.Map<GameCreatedDto>(game);
    }

    public async Task<GameDto> Get(Guid gameCode)
    {
        var game = await repository.GetAsync(x => x.Code == gameCode);

        return mapper.Map<GameDto>(game);
    }

    public async Task<IEnumerable<GameDto>> GetAll()
    {
        var games = await repository.GetManyAsync();

        return mapper.Map<IEnumerable<GameDto>>(games);
    }

    public async Task SetGameState(Guid code, GameState state)
    {
        await repository.SetStateAsync(code, state);
    }

    public async Task Join(Guid code, string playerName, string connectionId = null) // remove = null after removing http call
    {
        try
        {
            await gameHub.Groups.AddToGroupAsync(connectionId, code.ToString());
            await gameHub.Clients.Group(code.ToString()).SendAsync("PlayerConnected", playerName);
            await repository.ConnectPlayerAsync(code, playerName);

            if (await repository.AreBothPlayersConnectedAsync(code))
            {
                await repository.SetStateAsync(code, GameState.PlayersConnected); // todo: can we just skip playersConnected or there is something to do in between
                await gameHub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");
                await repository.SetStateAsync(code, GameState.Started);
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Game)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    proposedValues["IsBlackConnected"] = true;
                    proposedValues["IsWhiteConnected"] = true;
                    proposedValues["State"] = 2;
                    await gameHub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");
                    proposedValues["State"] = 3;

                    entry.OriginalValues.SetValues(databaseValues);
                    await entry.Context.SaveChangesAsync();
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
    }

    public async Task AddMove(Guid code, string move, string playerName)
    {
        var delimiter = ';';
        var moveTime = DateTime.UtcNow; // todo: should be client time? how to handle lags, cheating?

        // move will be validated already validated
        var game = await repository.GetAsync(x => x.Code  == code);
        ArgumentNullException.ThrowIfNull(game);

        if (game.State != GameState.Started)
            throw new GameMoveIncorrectStateException();

        var movesList = string.IsNullOrEmpty(game.Moves)
            ? []
            : game.Moves.Split(delimiter).SkipLast(1).ToList();

        if ((movesList.Count % 2 == 1 && playerName.Equals(game.BlackName, StringComparison.InvariantCultureIgnoreCase)) ||
            (movesList.Count % 2 == 0 && playerName.Equals(game.WhiteName, StringComparison.InvariantCultureIgnoreCase)))
            throw new GameMoveIncorrectPlayerException();

        if (!string.IsNullOrEmpty(game.Moves) && game.Moves.Contains($"{move}{delimiter}", StringComparison.InvariantCultureIgnoreCase))
            throw new GameMoveExistsException();

        // handle timer
        if (!game.BlackLastMoveTime.HasValue)
        {
            game.BlackLastMoveTime = moveTime;
            game.StartTime = moveTime;
        }
        else if (playerName.Equals(game.WhiteName, StringComparison.InvariantCultureIgnoreCase))
        {
            var timeLeft = TimeSpan.FromSeconds((double)game.WhiteTime) - (moveTime - game.BlackLastMoveTime);
            game.WhiteTime = (decimal)timeLeft.Value.TotalSeconds;
            game.WhiteLastMoveTime = moveTime;

            if (timeLeft.Value.TotalSeconds < 0)
            {
                game.State = GameState.FinishedByPlayerTimeout;
                game.Winner = game.BlackName;
            }
        }
        else if (playerName.Equals(game.BlackName, StringComparison.InvariantCultureIgnoreCase))
        {
            var timeLeft = TimeSpan.FromSeconds((double)game.BlackTime) - (moveTime - game.WhiteLastMoveTime);
            game.BlackTime = (decimal)timeLeft.Value.TotalSeconds;
            game.BlackLastMoveTime = moveTime;

            if (timeLeft.Value.TotalSeconds < 0)
            {
                game.State = GameState.FinishedByPlayerTimeout;
                game.Winner = game.WhiteName;
            }
        }

        if (game.State == GameState.FinishedByPlayerTimeout)
            await gameHub.Clients.Group(code.ToString()).SendAsync("GameFinishedByPlayerTimeout", game.Winner);
        else
        {
            game.Moves += move + delimiter;
            movesList.Add(move);
            await gameHub.Clients.Group(code.ToString()).SendAsync("MoveAdded", new MoveAdded
            {
                Move = move,
                Clock = new ClockDto
                {
                    Black = game.BlackTime,
                    White = game.WhiteTime
                }
            });

            var isWinningMove = IsWinningMove(move, movesList.Where((v, i) => i % 2 == (movesList.Count - 1) % 2));

            if (isWinningMove)
            {
                var blackWon = movesList.IndexOf(move) % 2 == 0;
                var winnerName = blackWon ? game.BlackName : game.WhiteName;
                await gameHub.Clients.Group(code.ToString()).SendAsync("GameFinished", winnerName);
                game.Winner = winnerName;
                game.State = GameState.Finished;
            }
        }

        await repository.UpdateAsync(game);
    }

    // todo: find better way to check wining move, this is extremly ugly
    private static bool IsWinningMove(string move, IEnumerable<string> moves)
    {
        var x = move[0];
        var y = int.Parse(move[1..]);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = 1; j >= -1; j--) // to scan top-bottom
            {
                if (i == 0 && j == 0)
                    continue;

                var count = 1 + CheckStonesInDirection(x, y, i, j, moves);

                if (count > 1 && count < 5) // todo: more than 6 is variant
                {
                    count += CheckStonesInDirection(x, y, 0 - i, 0 - j, moves);
                }

                if (count >= 5)
                    return true;
            }
        }

        return false;
    }

    private static int CheckStonesInDirection(char posX, int posY, int x, int y, IEnumerable<string> moves)
    {
        var count = 0;
        var moveToCheck = $"{(char)(posX + x)}{posY + y}";
        if ((posX + x) >= 'a' && (posX + x) <= 'o' && (posY + y) >= 1 && (posY + y) <= 15 && moves.Contains(moveToCheck))
            count = 1 + CheckStonesInDirection((char)(posX + x), posY + y, x, y, moves);

        return count;
    }
}
