using Microsoft.AspNetCore.Mvc;

namespace FootballHub.NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { Status = "Healthy", Service = "NotificationService" });
}