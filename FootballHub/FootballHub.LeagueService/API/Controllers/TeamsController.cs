using FootballHub.LeagueService.Application.DTOs;
using FootballHub.LeagueService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.LeagueService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ILeagueService _leagueService;

    public TeamsController(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request)
    {
        var result = await _leagueService.CreateTeamAsync(request);
        return result.Success ? Ok(result) : NotFound(result);
    }
}