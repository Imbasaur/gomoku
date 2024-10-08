﻿using AutoMapper;
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
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper, IWaitingListRepository waitingListRepository, IHubContext<GameHub> gameHub,
    GameTimeoutManager gameTimeoutManager, IServiceScopeFactory resolver) : IGameService
{
    private readonly char _movesDelimiter = ';';

    public async Task<GameCreatedDto> Create()
    {
        var players = await waitingListRepository.GetTop2Async();

        if (players == null || players.Count < 2)
            return null;

        var gameCode = Guid.NewGuid();

        var randomNumber = new Random().Next(0, 2);
        var gameTime = 300; // todo: should be configurable later
        var game = new Game
        {
            BlackName = players[randomNumber].PlayerName,
            WhiteName = players[randomNumber ^ 1].PlayerName,
            Code = gameCode,
            State = GameState.Created,
            Time = gameTime,
            BlackTime = gameTime,
            WhiteTime = gameTime,
        };

        await repository.AddAsync(game);
        await waitingListRepository.DeleteManyAsync(x => x.PlayerName == players[0].PlayerName || x.PlayerName == players[1].PlayerName);

        var gameInfo = mapper.Map<GameCreatedDto>(game);

        await gameHub.Clients.Client(players[0].ConnectionId).SendAsync("GameCreated", gameInfo);
        await gameHub.Clients.Client(players[1].ConnectionId).SendAsync("GameCreated", gameInfo); // how to handle observers, add them to gorup? is there any limit in groups?

        // todo: disconnect clients?

        return gameInfo;
    }

    public async Task<GameDto> Get(Guid gameCode)
    {
        var game = await repository.GetAsync(x => x.Code == gameCode);
        return mapper.Map<GameDto>(game);
    }

    public async Task<IEnumerable<GameListDto>> GetMany(Expression<Func<Game, bool>>? predicate = null)
    {
        var games = await repository.GetManyAsync(predicate);

        return mapper.Map<IEnumerable<GameListDto>>(games);
    }

    public async Task SetGameState(Guid code, GameState state)
    {
        await repository.SetStateAsync(code, state);
    }

    public async Task Join(Guid code, string playerName, string connectionId = null, bool asObserver = false) // remove = null after removing http call
    {
        try
        {
            await gameHub.Groups.AddToGroupAsync(connectionId, code.ToString());

            if (!asObserver)
            {
                await gameHub.Clients.Group(code.ToString()).SendAsync("PlayerConnected", playerName); // todo: change all hub messages to consts
                await repository.ConnectPlayerAsync(code, playerName);

                if (await repository.AreBothPlayersConnectedAsync(code))
                {
                    await repository.SetStateAsync(code, GameState.PlayersConnected);
                    await gameHub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");
                }
            }
            else
            {
                await gameHub.Clients.Group(code.ToString()).SendAsync("ObserverConnected"); // todo: change all hub messages to consts
                var game = await repository.GetAsync(x => x.Code == code);
                
                await gameHub.Clients.Client(connectionId).SendAsync("InitGame", mapper.Map<InitGame>(game));
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
        var moveTime = DateTime.UtcNow; // todo: should be client time? how to handle lags, cheating?

        // todo: validate move (range, space on board)

        var game = await repository.GetAsync(x => x.Code == code);
        ArgumentNullException.ThrowIfNull(game);

        var movesList = string.IsNullOrEmpty(game.Moves)
            ? []
            : game.Moves.Split(_movesDelimiter).SkipLast(1).ToList();

        if ((movesList.Count % 2 == 1 && playerName.Equals(game.BlackName, StringComparison.InvariantCultureIgnoreCase)) ||
            (movesList.Count % 2 == 0 && playerName.Equals(game.WhiteName, StringComparison.InvariantCultureIgnoreCase)))
            throw new GameMoveIncorrectPlayerException();

        if (!string.IsNullOrEmpty(game.Moves) && game.Moves.Contains($"{move}{_movesDelimiter}", StringComparison.InvariantCultureIgnoreCase))
            throw new GameMoveExistsException();

        if (game.State == GameState.PlayersConnected)
        {
            game.State = GameState.Started;
            await gameHub.Clients.Group("watchlist").SendAsync("GameStarted", new GameListDto(game.Code, game.BlackName, game.WhiteName));
        }

        if (game.State != GameState.Started)
            throw new GameMoveIncorrectStateException();

        // handle timer
        var activePlayer = (PlayerColor)(movesList.Count % 2);
        if (!game.BlackLastMoveTime.HasValue)
        {
            game.BlackLastMoveTime = moveTime;
            game.StartTime = moveTime;
        }
        else
            CheckTimeoutWinCondition(activePlayer, moveTime, game);

        if (game.State == GameState.FinishedByPlayerTimeout)
            await gameHub.Clients.Group(code.ToString()).SendAsync("GameFinishedByPlayerTimeout", game.Winner);
        else
        {
            game.Moves += move + _movesDelimiter;
            movesList.Add(move);
            await gameHub.Clients.Group(code.ToString()).SendAsync("MoveAdded", new MoveAdded
            {
                Move = move,
                Clock = new ClockDto
                {
                    Black = decimal.Round(game.BlackTime, 2),
                    White = decimal.Round(game.WhiteTime, 2)
                }
            });

            var winningStones = GetWinningStones(move, movesList.Where((v, i) => i % 2 == (movesList.Count - 1) % 2));

            if (!string.IsNullOrEmpty(winningStones))
            {
                var blackWon = movesList.IndexOf(move) % 2 == 0;
                var winnerName = blackWon ? game.BlackName : game.WhiteName;
                gameTimeoutManager.StopTracking(code);
                await gameHub.Clients.Groups(code.ToString(), "watchlist").SendAsync("GameFinished", new GameFinished
                {
                    Code = code.ToString(),
                    Winner = winnerName,
                    WinningStones = [.. winningStones.Split(';')]
                });
                game.Winner = winnerName;
                game.State = GameState.Finished;
            }
            else
                gameTimeoutManager.TrackGameTimeout(code, activePlayer == PlayerColor.Black ? game.WhiteTime : game.BlackTime, GetCheckTimeoutFunc());
        }

        await repository.UpdateAsync(game);
    }

    public async Task FinishGamesByDisconnect(string playerName)
    {
        var games = await repository.GetManyAsync(x => x.BlackName == playerName || x.WhiteName == playerName);

        foreach (var game in games)
        {
            gameTimeoutManager.StopTracking(game.Code);
            game.Winner = playerName == game.WhiteName ? game.BlackName : game.WhiteName;
            game.State = GameState.FinishedByPlayerDisconnect;
            await repository.UpdateAsync(game);
            await gameHub.Clients.Groups(game.Code.ToString(), "watchlist").SendAsync("GameFinishedByPlayerDisconnect", new GameFinished
            {
                Code = game.Code.ToString(),
                Winner = game.Winner
            });
        }
    }

    public async Task CheckTimeoutWin(Guid gameCode)
    {
        var currentTime = DateTime.UtcNow;
        var game = await repository.GetAsync(x => x.Code == gameCode);
        ArgumentNullException.ThrowIfNull(game);

        var movesList = string.IsNullOrEmpty(game.Moves)
            ? []
            : game.Moves.Split(_movesDelimiter).SkipLast(1).ToList();
        var activePlayer = (PlayerColor)(movesList.Count % 2);

        if (CheckTimeoutWinCondition(activePlayer, currentTime, game))
        {
            await gameHub.Clients.Groups(gameCode.ToString(), "watchlist").SendAsync("GameFinishedByPlayerTimeout", new GameFinished
            {
                Code = game.Code.ToString(),
                Winner = game.Winner
            });
            await repository.UpdateAsync(game);
        }
    }

    public Func<Guid, Task> GetCheckTimeoutFunc()
    {
        return async (gameId) =>
        {
            using var scope = resolver.CreateAsyncScope();
            var scopedGameService = scope.ServiceProvider.GetRequiredService<IGameService>();
            await scopedGameService.CheckTimeoutWin(gameId);
        };
    }

    private static bool CheckTimeoutWinCondition(PlayerColor activePlayer, DateTime actionTime, Game game)
    {
        if (activePlayer == PlayerColor.White)
        {
            var timeLeft = TimeSpan.FromSeconds((double)game.WhiteTime) - (actionTime - game.BlackLastMoveTime);
            game.WhiteTime = (decimal)timeLeft.Value.TotalSeconds;
            game.WhiteLastMoveTime = actionTime;

            if (timeLeft.Value.TotalSeconds < 0)
            {
                game.State = GameState.FinishedByPlayerTimeout;
                game.Winner = game.BlackName;

                return true;
            }
        }
        else
        {
            var timeLeft = TimeSpan.FromSeconds((double)game.BlackTime) - (actionTime - game.WhiteLastMoveTime);
            game.BlackTime = (decimal)timeLeft.Value.TotalSeconds;
            game.BlackLastMoveTime = actionTime;

            if (timeLeft.Value.TotalSeconds < 0)
            {
                game.State = GameState.FinishedByPlayerTimeout;
                game.Winner = game.WhiteName;

                return true;
            }
        }

        return false;
    }

    // todo: find better way to check wining move, this is extremly ugly
    private static string GetWinningStones(string move, IEnumerable<string> moves)
    {
        var x = move[0];
        var y = int.Parse(move[1..]);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = 1; j >= -1; j--) // to scan top-bottom
            {
                if (i == 0 && j == 0)
                    continue;

                var winningStones = GetWinningStonesInDriection(x, y, i, j, moves);

                if (!string.IsNullOrEmpty(winningStones) && winningStones.Split(";").Length < 5) // todo: more than 6 is variant
                {
                    winningStones += GetWinningStonesInDriection(x, y, 0 - i, 0 - j, moves);
                }
                winningStones += move;

                if (winningStones.Split(";").Length >= 5)
                    return winningStones.TrimEnd(';');
            }
        }

        return string.Empty;
    }

    private static int CheckStonesInDirection(char posX, int posY, int x, int y, IEnumerable<string> moves)
    {
        var count = 0;
        var moveToCheck = $"{(char)(posX + x)}{posY + y}";
        if ((posX + x) >= 'a' && (posX + x) <= 'o' && (posY + y) >= 1 && (posY + y) <= 15 && moves.Contains(moveToCheck))
            count = 1 + CheckStonesInDirection((char)(posX + x), posY + y, x, y, moves);

        return count;
    }

    private static string GetWinningStonesInDriection(char posX, int posY, int x, int y, IEnumerable<string> moves)
    {
        var winningStones = string.Empty;
        var moveToCheck = $"{(char)(posX + x)}{posY + y}";
        if ((posX + x) >= 'a' && (posX + x) <= 'o' && (posY + y) >= 1 && (posY + y) <= 15 && moves.Contains(moveToCheck))
            winningStones = moveToCheck + ";" + GetWinningStonesInDriection((char)(posX + x), posY + y, x, y, moves);

        return winningStones;
    }
}
