using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.FileServerAPI.Controllers;

[ApiController]
[Route("api/health-check")]
public class HealthCheckController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult PingServer()
    {
        return Content("Pong!");
    }
}