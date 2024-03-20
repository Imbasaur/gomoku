﻿using Microsoft.AspNetCore.SignalR;

namespace Gomoku.Api.Hubs;

public class GameHub(ILogger<GameHub> logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId} {Context.UserIdentifier}");

        return base.OnConnectedAsync();
    }
}
