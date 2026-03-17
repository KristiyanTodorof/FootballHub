using FootballHub.Shared.Entities;

namespace FootballHub.PlayerService.Domain.Entities;

public class Player : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int? ShirtNumber { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int LeagueId { get; set; }

    public ICollection<PlayerStatistic> Statistics { get; set; } = [];
}