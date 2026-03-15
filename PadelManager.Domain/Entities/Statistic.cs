using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Statistic : BaseEntity
    {
        public int Points { get; set; }
        public int WoCount { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }

        // Relationships FK
        public Guid CoupleId { get; set; }
        public Guid ZoneId { get; set; }

        // Navigation properties
        public Couple Couple { get; set; } = null!;
        public Zone Zone { get; set; } = null!;
    }
}
