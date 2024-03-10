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
}
