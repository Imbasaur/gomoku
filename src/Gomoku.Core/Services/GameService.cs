using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Exceptions;
using Gomoku.Core.Hubs;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper, IWaitingListRepository waitingListRepository, IHubContext<GameHub> hub) : IGameService
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
            BlackName = players[randomNumber], // todo: have to add some randomisation here
            WhiteName = players[randomNumber ^ 1],
            Code = gameCode,
            State = GameState.Created
        };

        await repository.AddAsync(game);
        await waitingListRepository.DeleteManyAsync(x => x.PlayerName == players[0] || x.PlayerName == players[1]);

        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.WhiteName); // todo: remove this, don't really need to update waitingList on frontend side
        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.BlackName);

        await hub.Groups.AddToGroupAsync(game.WhiteName, game.Code.ToString());
        await hub.Groups.AddToGroupAsync(game.BlackName, game.Code.ToString());
        await hub.Clients.Group(game.Code.ToString()).SendAsync("GameCreated", game.Code); // how to handle observers, add them to gorup? is there any limit in groups?

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

    public async Task Join(Guid code, string playerName)
    {
        try
        {
            await repository.ConnectPlayerAsync(code, playerName);
            await hub.Clients.Group(code.ToString()).SendAsync("PlayerConnected", playerName);

            if (await repository.AreBothPlayersConnectedAsync(code))
            {
                await repository.SetStateAsync(code, GameState.PlayersConnected);
                await hub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");
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
                    await hub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");

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

    public async Task AddMove(Guid code, string move)
    {
        var game = await repository.GetAsync(x => x.Code  == code);
        ArgumentNullException.ThrowIfNull(game);

        if (!string.IsNullOrEmpty(game.Moves) && game.Moves.Contains(move, StringComparison.InvariantCultureIgnoreCase))
            throw new GameMoveExistsException();

        // todo: add move verification (player, color)  

        await repository.AddMoveAsync(code, move);

        await hub.Clients.Group(code.ToString()).SendAsync("MoveAdded", move);
    }
}
