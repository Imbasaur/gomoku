using Gomoku.Core.Requests;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Enums;
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
        logger.LogInformation($"New client connected to {nameof(GameHub)} with connectionId {Context.ConnectionId}, username: {username}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await waitingListService.Remove(Context.ConnectionId);
        await gameService.FinishGamesByDisconnect(Context.GetHttpContext().Request.Query["username"]);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task<bool> JoinWaitingList()
    {
        var username = Context.GetHttpContext().Request.Query["username"]; // todo: remove when accounts will be added
        await waitingListService.Add(username, Context.ConnectionId);

        return true;
    }

    public async Task<bool> LeaveWaitingList()
    {
        await waitingListService.Remove(Context.ConnectionId);

        return true;
    }

    public async Task<bool> JoinGame(JoinGameRequest request)
    {
        await gameService.Join(request.Code, request.PlayerName, Context.ConnectionId, request.AsObserver);

        return true;
    }

    public async Task<bool> Move(AddMoveRequest request)
    {
        var username = Context.GetHttpContext().Request.Query["username"]; // todo: remove when accounts will be added
        await gameService.AddMove(request.Code, request.Move, username);

        return true;
    }

    public async Task<bool> SubscribeGroup(string group)
    {
        // todo: define groups somewhere
        if (group.Equals("watchlist", StringComparison.InvariantCultureIgnoreCase))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Client(Context.ConnectionId).SendAsync("ActiveGames", await gameService.GetMany(x => x.State == GameState.Started));

            return true;
        }

        return false;
    }
}
