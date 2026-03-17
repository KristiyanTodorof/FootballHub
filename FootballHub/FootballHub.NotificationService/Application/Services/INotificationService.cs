namespace FootballHub.NotificationService.Application.Services;

public interface INotificationService
{
    Task SendMatchStartedAsync(int matchId, string homeTeam, string awayTeam);
    Task SendGoalScoredAsync(int matchId, string team, string playerName, int minute, int homeScore, int awayScore);
    Task SendMatchFinishedAsync(int matchId, int homeScore, int awayScore);
}