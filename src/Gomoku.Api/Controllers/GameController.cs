using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        return Ok(await gameService.CreateGame());
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        return Ok(await gameService.GetGames());
    }

    [HttpGet]
    [Route("{code}")]
    public async Task<IActionResult> GetGame(Guid code)
    {
        return Ok(await gameService.GetGame(code));
    }
}
