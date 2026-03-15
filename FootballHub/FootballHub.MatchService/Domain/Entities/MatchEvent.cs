using FootballHub.Shared.Entities;

namespace FootballHub.MatchService.Domain.Entities
{
    public class MatchEvent : BaseEntity
    {
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public MatchEventType Type { get; set; }
        public int Minute { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string? AssistPlayerName { get; set; }
        public string Team { get; set; } = string.Empty;
    }
    public enum MatchEventType
    {
        Goal,
        OwnGoal,
        YellowCard,
        RedCard,
        Substitution,
        PenaltyMissed
    }
}
