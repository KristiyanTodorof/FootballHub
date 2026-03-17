namespace FootballHub.PlayerService.Application.DTOs;

public record PlayerDto(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    DateTime DateOfBirth,
    int Age,
    string Nationality,
    string Position,
    int? ShirtNumber,
    string PhotoUrl,
    int TeamId,
    string TeamName,
    List<PlayerStatisticDto> Statistics
);

public record PlayerStatisticDto(
    int Season,
    string LeagueName,
    int Appearances,
    int Goals,
    int Assists,
    int YellowCards,
    int RedCards,
    int MinutesPlayed
);

public record CreatePlayerRequest(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    string Position,
    int? ShirtNumber,
    string PhotoUrl,
    int TeamId,
    string TeamName,
    int LeagueId
);

public record UpdatePlayerStatisticRequest(
    int Season,
    int LeagueId,
    string LeagueName,
    int Appearances,
    int Goals,
    int Assists,
    int YellowCards,
    int RedCards,
    int MinutesPlayed
);