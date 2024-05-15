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

    public override async Task OnConnectedAsync()
    {
        var username = Context.GetHttpContext().Request.Query["username"]; // todo: remove when accounts will be added
        await waitingListService.Add(username, Context.ConnectionId);
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId}, username: {username}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await waitingListService.Remove(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task<bool> Join(JoinGameRequest request)
    {
        await gameService.Join(request.Code, request.PlayerName, Context.ConnectionId);

        return true;
    }

    public async Task<bool> Move(AddMoveRequest request)
    {
        await gameService.AddMove(request.Code, request.Move);

        return true;
    }
}
