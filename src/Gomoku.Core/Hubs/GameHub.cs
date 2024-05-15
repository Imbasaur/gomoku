using Gomoku.Core.Requests;
using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Gomoku.Core.Hubs;

public class GameHub(ILogger<GameHub> logger, IGameService gameService, IWaitingListService waitingListService) : Hub
{
    public async Task SendMessageAsync(string name, string message)
    {
        await Clients.All.SendAsync(name, message);
    }

    public override Task OnConnectedAsync()
    {
        var username = Context.GetHttpContext().Request.Query["username"];
        waitingListService.Add(username, Context.ConnectionId);
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId}, username: {username}");

        return base.OnConnectedAsync();
    }
}
