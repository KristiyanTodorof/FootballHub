using FootballHub.Shared.Entities;

namespace FootballHub.LeagueService.Domain.Entities;

public class League : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public int Season { get; set; }

    public ICollection<Team> Teams { get; set; } = [];
    public ICollection<Standing> Standings { get; set; } = [];
}