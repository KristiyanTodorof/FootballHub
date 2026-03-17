using FootballHub.LeagueService.Application.DTOs;
using FootballHub.LeagueService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.LeagueService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly ILeagueService _leagueService;

    public LeaguesController(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeagues()
        => Ok(await _leagueService.GetAllLeaguesAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLeague(int id)
    {
        var result = await _leagueService.GetLeagueByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeague([FromBody] CreateLeagueRequest request)
    {
        var result = await _leagueService.CreateLeagueAsync(request);
        return CreatedAtAction(nameof(GetLeague), new { id = result.Data!.Id }, result);
    }

    [HttpGet("{id:int}/standings")]
    public async Task<IActionResult> GetStandings(int id)
    {
        var result = await _leagueService.GetStandingsAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}/standings")]
    public async Task<IActionResult> UpdateStanding(int id, [FromBody] UpdateStandingRequest request)
    {
        var result = await _leagueService.UpdateStandingAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }
}