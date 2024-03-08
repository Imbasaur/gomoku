using Microsoft.AspNetCore.Mvc;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogDebug("test");

        return Ok("test");
    }
}
