﻿using AutoMapper;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.Core.Hubs;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Repository;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Gomoku.Core.Services;
public class WaitingListService(IWaitingListRepository repository, IMapper mapper, IHubContext<GameHub> hub, IGameService gameService) : IWaitingListService
{
    public async Task Add(string playerName, string connectionId)
    {
        await repository.AddAsync(new PlayerWaiting(playerName, connectionId));

        await hub.Clients.All.SendAsync("PlayerJoinedWaitingList", playerName); // remove later, no need to send this to front

        if (await repository.CountAsync() > 1)
            await gameService.Create();

        return;
    }

    public async Task<int> Count()
    {
        return await repository.CountAsync();
    }

    public async Task<IEnumerable<PlayerWaitingDto>> GetAll()
    {
        var players = await repository.GetManyAsync();
        await hub.Clients.All.SendAsync("PlayersInQueue", JsonSerializer.Serialize(players.Select(x => x.PlayerName)));

        return mapper.Map<IEnumerable<PlayerWaitingDto>>(players);
    }

    public async Task Remove(string connectionId)
    {
        if (string.IsNullOrEmpty(connectionId))
            return;

        try
        {
            await repository.DeleteAsync(x => x.ConnectionId.ToLower().Equals(connectionId.ToLower())); // this will be account id in future

            await hub.Clients.All.SendAsync("PlayerLeftWaitingList", connectionId);
        }
        catch (Exception ex) 
        {
            // if DeleteAsync fails, then we can continue without sending hub message
        }
    }
}
