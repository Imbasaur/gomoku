using Gomoku.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Gomoku.Api.Hubs;

public class GameHub(ILogger<GameHub> logger) : Hub<IGameHub>
{
    public async Task SendMessageAsync(string name, string message)
    {
        await Clients.All.SendMessageAsync(name, message);
    }

    public override Task OnConnectedAsync()
    {
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId} {Context.UserIdentifier}");

        return base.OnConnectedAsync();
    }
}
