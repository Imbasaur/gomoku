using Gomoku.Api.Hubs;
using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService, IHubContext<GameHub> hub) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        var game = await gameService.Create();

        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.WhiteName); // todo: remove this, don't really need to update waitingList on frontend side
        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", game.BlackName);

        await hub.Clients.Clients(game.BlackName, game.WhiteName).SendAsync("GameCreated", game); // maybe create group per game? how to list game for observers?

        return Ok(game);
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        return Ok(await gameService.GetAll());
    }

    [HttpGet]
    [Route("{code}")]
    public async Task<IActionResult> GetGame(Guid code)
    {
        return Ok(await gameService.Get(code));
    }
}
