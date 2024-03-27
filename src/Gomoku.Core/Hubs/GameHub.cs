using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Gomoku.Api.Hubs;

public class GameHub(ILogger<GameHub> logger) : Hub
{
    public async Task SendMessageAsync(string name, string message)
    {
        await Clients.All.SendAsync(name, message);
    }

    public override Task OnConnectedAsync()
    {
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId} {Context.UserIdentifier}");

        return base.OnConnectedAsync();
    }
}
