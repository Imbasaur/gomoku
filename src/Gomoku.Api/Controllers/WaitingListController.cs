using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WaitingListController(IWaitingListService waitingListService) : Controller
{
    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetGames()
    {
        return Ok(await waitingListService.Count());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await waitingListService.GetAll());
    }
}
