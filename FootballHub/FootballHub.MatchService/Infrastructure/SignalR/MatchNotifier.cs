using FootballHub.MatchService.API.Hubs;
using FootballHub.MatchService.Application.DTOs;
using FootballHub.MatchService.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FootballHub.MatchService.Infrastructure.SignalR;

public class MatchNotifier : IMatchNotifier
{
    private readonly IHubContext<MatchHub> _hubContext;

    public MatchNotifier(IHubContext<MatchHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyMatchStartedAsync(MatchDto match)
    {
        await _hubContext.Clients
            .Group("live_matches")
            .SendAsync("MatchStarted", match);

        await _hubContext.Clients
            .Group($"match_{match.Id}")
            .SendAsync("MatchStarted", match);
    }

    public async Task NotifyGoalScoredAsync(MatchDto match, MatchEventDto goal)
    {
        var payload = new
        {
            Match = match,
            Goal = goal
        };

        await _hubContext.Clients
            .Group("live_matches")
            .SendAsync("GoalScored", payload);

        await _hubContext.Clients
            .Group($"match_{match.Id}")
            .SendAsync("GoalScored", payload);
    }

    public async Task NotifyScoreUpdatedAsync(MatchDto match)
    {
        await _hubContext.Clients
            .Group("live_matches")
            .SendAsync("ScoreUpdated", new
            {
                MatchId = match.Id,
                match.HomeTeamName,
                match.AwayTeamName,
                match.HomeScore,
                match.AwayScore,
                match.Minute,
                match.Status
            });
    }

    public async Task NotifyMatchFinishedAsync(MatchDto match)
    {
        await _hubContext.Clients
            .Group("live_matches")
            .SendAsync("MatchFinished", match);

        await _hubContext.Clients
            .Group($"match_{match.Id}")
            .SendAsync("MatchFinished", match);
    }

    public async Task NotifyMatchEventAsync(MatchDto match, MatchEventDto matchEvent)
    {
        await _hubContext.Clients
            .Group($"match_{match.Id}")
            .SendAsync("MatchEvent", new
            {
                MatchId = match.Id,
                Event = matchEvent
            });
    }
}