namespace FootballHub.LeagueService.Application.DTOs;

public record LeagueDto(
    int Id,
    string Name,
    string Country,
    string LogoUrl,
    int Season,
    List<TeamDto> Teams
);

public record TeamDto(
    int Id,
    string Name,
    string ShortName,
    string Country,
    string LogoUrl
);

public record StandingDto(
    int Position,
    int TeamId,
    string TeamName,
    string TeamShortName,
    string TeamLogoUrl,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points
);

public record CreateLeagueRequest(
    string Name,
    string Country,
    string LogoUrl,
    int Season
);

public record CreateTeamRequest(
    string Name,
    string ShortName,
    string Country,
    string LogoUrl,
    int LeagueId
);

public record UpdateStandingRequest(
    int TeamId,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst
);