using FootballHub.Shared.Entities;

namespace FootballHub.LeagueService.Domain.Entities;

public class Standing : BaseEntity
{
    public int LeagueId { get; set; }
    public League League { get; set; } = null!;
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public int Position { get; set; }
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => (Won * 3) + Drawn;
}