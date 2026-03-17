namespace FootballHub.NotificationService.Application.Services;

public class NotificationAppService : INotificationService
{
    private readonly ILogger<NotificationAppService> _logger;

    public NotificationAppService(ILogger<NotificationAppService> logger)
    {
        _logger = logger;
    }

    public async Task SendMatchStartedAsync(int matchId, string homeTeam, string awayTeam)
    {
        _logger.LogInformation(
            "Мач започна: {HomeTeam} vs {AwayTeam} (MatchId: {MatchId})",
            homeTeam, awayTeam, matchId);
        await Task.CompletedTask;
    }

    public async Task SendGoalScoredAsync(
        int matchId,
        string team,
        string playerName,
        int minute,
        int homeScore,
        int awayScore)
    {
        _logger.LogInformation(
            "ГОЛ! {PlayerName} ({Team}) {Minute}' | Резултат: {HomeScore}-{AwayScore} (MatchId: {MatchId})",
            playerName, team, minute, homeScore, awayScore, matchId);

        await Task.CompletedTask;
    }

    public async Task SendMatchFinishedAsync(int matchId, int homeScore, int awayScore)
    {
        _logger.LogInformation(
            "Мачът приключи: {HomeScore}-{AwayScore} (MatchId: {MatchId})",
            homeScore, awayScore, matchId);

        await Task.CompletedTask;
    }
}