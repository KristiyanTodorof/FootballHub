using FootballHub.Shared.Entities;

namespace FootballHub.MatchService.Domain
{
    public class Match : BaseEntity
    {
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = string.Empty;
        public DateTime KickOff { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int? Minute { get; set; }

        public ICollection<MatchEvent> Events { get; set; } = [];
    }
    public enum MatchStatus
    {
        Scheduled,
        Live,
        HalfTime,
        Finished,
        Postponed,
        Cancelled
    }
}
