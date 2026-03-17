using FootballHub.MatchService.Application.DTOs;

namespace FootballHub.MatchService.Application.Interfaces;

public interface IMatchNotifier
{
    Task NotifyMatchStartedAsync(MatchDto match);
    Task NotifyGoalScoredAsync(MatchDto match, MatchEventDto goal);
    Task NotifyScoreUpdatedAsync(MatchDto match);
    Task NotifyMatchFinishedAsync(MatchDto match);
    Task NotifyMatchEventAsync(MatchDto match, MatchEventDto matchEvent);
}