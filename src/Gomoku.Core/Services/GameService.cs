using AutoMapper;
using Gomoku.Api.Hubs;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper, IWaitingListRepository waitingListRepository, IHubContext<GameHub> hub) : IGameService
{
    public async Task<GameCreatedDto> Create()
    {
        var players = waitingListRepository.GetTop2();

        if (players == null || players.Count() < 2)
            return null;

        var gameCode = Guid.NewGuid();
        var game = new Game
        {
            BlackName = players[0], // todo: have to add some randomisation here
            WhiteName = players[1],
            Code = gameCode,
            State = GameState.Created
        };

        repository.Add(game);
        waitingListRepository.DeleteMany(x => x.PlayerName == players[0] || x.PlayerName == players[1]);

        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.WhiteName); // todo: remove this, don't really need to update waitingList on frontend side
        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.BlackName);

        await hub.Clients.Clients(game.BlackName, game.WhiteName).SendAsync("GameCreated", game); // maybe create group per game? how to list game for observers?
        await hub.Clients.All.SendAsync("GameCreatedAll", game); // maybe create group per game? how to list game for observers?

        return mapper.Map<GameCreatedDto>(game);
    }

    public Task<GameDto> Get(Guid gameCode)
    {
        var game = repository.Get(x => x.Code == gameCode);

        return Task.FromResult(mapper.Map<GameDto>(game));
    }

    public Task<IEnumerable<GameDto>> GetAll()
    {
        var games = repository.GetMany();

        return Task.FromResult(mapper.Map<IEnumerable<GameDto>>(games));
    }

    public Task SetGameState(Guid code, GameState state)
    {
        repository.SetState(code, state);

        return Task.CompletedTask;
    }
}
