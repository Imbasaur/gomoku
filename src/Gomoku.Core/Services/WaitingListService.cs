using AutoMapper;
using Gomoku.Api.Hubs;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Gomoku.Core.Services;
public class WaitingListService(IWaitingListRepository repository, IMapper mapper, IHubContext<GameHub> hub, IGameService gameService) : IWaitingListService
{
    public async Task Add(string playerName)
    {
        repository.Add(new PlayerWaiting(playerName));

        await hub.Clients.All.SendAsync("PlayerJoinedWaitingList", playerName); // remove later, no need to send this to front

        if (repository.Count() > 1)
            await gameService.Create();

        return;
    }

    public Task<int> Count()
    {
        return Task.FromResult(repository.Count());
    }

    public async Task<IEnumerable<PlayerWaitingDto>> GetAll()
    {
        var players = repository.GetMany();
        await hub.Clients.All.SendAsync("PlayersInQueue", players.ToString());

        return mapper.Map<IEnumerable<PlayerWaitingDto>>(players);
    }

    public async Task Remove(string playerName)
    {
        repository.Delete(x => x.PlayerName.ToLower().Equals(playerName.ToLower())); // this will be id in future

        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", playerName);

        return;
    }
}
