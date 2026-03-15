using FootballHub.Shared.Entities;

namespace FootballHub.LeagueService.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public int LeagueId { get; set; }
    public League League { get; set; } = null!;

    public ICollection<Standing> Standings { get; set; } = [];
}