using FootballHub.Shared.Entities;

namespace FootballHub.PlayerService.Domain.Entities;

public class PlayerStatistic : BaseEntity
{
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public int Season { get; set; }
    public int LeagueId { get; set; }
    public string LeagueName { get; set; } = string.Empty;
    public int Appearances { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int MinutesPlayed { get; set; }
}