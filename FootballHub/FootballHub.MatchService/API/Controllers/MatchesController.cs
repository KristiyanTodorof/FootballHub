using FootballHub.MatchService.Application.DTOs;
using FootballHub.MatchService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.MatchService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysMatches()
        => Ok(await _matchService.GetTodaysMatchesAsync());

    [HttpGet("live")]
    public async Task<IActionResult> GetLiveMatches()
        => Ok(await _matchService.GetLiveMatchesAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMatch(int id)
    {
        var result = await _matchService.GetMatchByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
    {
        var result = await _matchService.CreateMatchAsync(request);
        return CreatedAtAction(nameof(GetMatch), new { id = result.Data!.Id }, result);
    }

    [HttpPost("{id:int}/start")]
    public async Task<IActionResult> StartMatch(int id)
    {
        var result = await _matchService.StartMatchAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}/score")]
    public async Task<IActionResult> UpdateScore(int id, [FromBody] UpdateScoreRequest request)
    {
        var result = await _matchService.UpdateScoreAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost("{id:int}/events")]
    public async Task<IActionResult> AddMatchEvent(int id, [FromBody] AddMatchEventRequest request)
    {
        var result = await _matchService.AddMatchEventAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost("{id:int}/finish")]
    public async Task<IActionResult> FinishMatch(int id)
    {
        var result = await _matchService.FinishMatchAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}