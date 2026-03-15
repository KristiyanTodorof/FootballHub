using FootballHub.MatchService.Domain.Entities;

namespace FootballHub.MatchService.Application.DTOs;

public record MatchDto(
    int Id,
    string HomeTeamName,
    string AwayTeamName,
    string LeagueName,
    DateTime KickOff,
    string Status,
    int HomeScore,
    int AwayScore,
    int? Minute,
    List<MatchEventDto> Events
);

public record MatchEventDto(
    int Id,
    string Type,
    int Minute,
    string PlayerName,
    string? AssistPlayerName,
    string Team
);

public record CreateMatchRequest(
    int HomeTeamId,
    string HomeTeamName,
    int AwayTeamId,
    string AwayTeamName,
    int LeagueId,
    string LeagueName,
    DateTime KickOff
);

public record AddMatchEventRequest(
    MatchEventType Type,
    int Minute,
    int PlayerId,
    string PlayerName,
    string? AssistPlayerName,
    string Team
);

public record UpdateScoreRequest(
    int HomeScore,
    int AwayScore,
    int Minute
);