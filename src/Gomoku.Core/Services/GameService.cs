using AutoMapper;
using Gomoku.Api.Hubs;
using Gomoku.Core.Dtos.Games;
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
        var game = new Game
        {
            BlackName = players[0], // todo: have to add some randomisation here
            WhiteName = players[1],
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

    public Task<GameDto> Get(Guid gameCode)
    {
        var game = repository.GetAsync(x => x.Code == gameCode);

        return Task.FromResult(mapper.Map<GameDto>(game));
    }

    public Task<IEnumerable<GameDto>> GetAll()
    {
        var games = repository.GetManyAsync();

        return Task.FromResult(mapper.Map<IEnumerable<GameDto>>(games));
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

            if (await repository.AreBothPlayersConnectedAsync(code))
            {
                await repository.SetStateAsync(code, GameState.PlayersConnected);
                await hub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");
            }
            else
                await hub.Clients.Group(code.ToString()).SendAsync("PlayerConnected", playerName);
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
                    await repository.SetStateAsync(code, GameState.PlayersConnected);
                    await hub.Clients.Group(code.ToString()).SendAsync("PlayersConnected");

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
