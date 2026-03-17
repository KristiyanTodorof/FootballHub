using FootballHub.PlayerService.Application.DTOs;
using FootballHub.PlayerService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.PlayerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPlayer(int id)
    {
        var result = await _playerService.GetPlayerByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("team/{teamId:int}")]
    public async Task<IActionResult> GetPlayersByTeam(int teamId)
        => Ok(await _playerService.GetPlayersByTeamAsync(teamId));

    [HttpGet("top-scorers")]
    public async Task<IActionResult> GetTopScorers([FromQuery] int leagueId, [FromQuery] int season)
        => Ok(await _playerService.GetTopScorersAsync(leagueId, season));

    [HttpGet("top-assists")]
    public async Task<IActionResult> GetTopAssists([FromQuery] int leagueId, [FromQuery] int season)
        => Ok(await _playerService.GetTopAssistsAsync(leagueId, season));

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        var result = await _playerService.CreatePlayerAsync(request);
        return CreatedAtAction(nameof(GetPlayer), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id:int}/statistics")]
    public async Task<IActionResult> UpdateStatistic(int id, [FromBody] UpdatePlayerStatisticRequest request)
    {
        var result = await _playerService.UpdateStatisticAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }
}