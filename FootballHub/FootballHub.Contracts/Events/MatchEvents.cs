using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballHub.Contracts.Events
{
    public class MatchEvents
    {
        public record MatchStarted
        {
            public int MatchId { get; init; }
            public string HomeTeam { get; init; } = string.Empty;
            public string AwayTeam { get; init; } = string.Empty;
            public DateTime KickOff { get; init; }
        }

        public record GoalScored
        {
            public int MatchId { get; init; }
            public string Team { get; init; } = string.Empty;
            public string PlayerName { get; init; } = string.Empty;
            public int Minute { get; init; }
            public int HomeScore { get; init; }
            public int AwayScore { get; init; }
        }

        public record MatchFinished
        {
            public int MatchId { get; init; }
            public int HomeScore { get; init; }
            public int AwayScore { get; init; }
            public DateTime FinishedAt { get; init; }
        }
    }
}
