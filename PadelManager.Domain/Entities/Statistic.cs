using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Statistic : BaseEntity
    {
        public required int Points { get; set; }

        public required int WoCount { get; set; }

        public required int MatchesPlayed { get; set; }

        public required int MatchesWon { get; set; }

        public required string SetsWon { get; set; }

        public required string SetsLost { get; set; }

        public required int GamesWon { get; set; }

        public required int GamesLost { get; set; }

        // Relationships FK
        public Guid CoupleId { get; set; }

        public Guid ZoneId { get; set; }
        
        // Navigation properties
        public required Couple Couple { get; set; }

        public required Zone Zone { get; set; }
    }
}
