using Gomoku.Core.Requests;
using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        var game = await gameService.Create();

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

    [HttpPut]
    [Route("join")]
    public async Task<IActionResult> JoinGame(JoinGameRequest request) // todo: change to caller identity
    {
        await gameService.Join(request.Code, request.PlayerName);

        return Ok();
    }

    [HttpPost]
    [Route("move")]
    public async Task<IActionResult> AddMove(AddMoveRequest request)
    {
        await gameService.AddMove(request.Code, request.Move);

        return Ok();
    }
}
