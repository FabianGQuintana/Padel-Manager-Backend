using System;

namespace PadelManager.Application.DTOs.Statistic
{
    public class UpdateStatisticDto
    {
        public required Guid Id { get; set; }

        public Guid? CoupleId { get; set; }

        public Guid? ZoneId { get; set; }

        public int? Points { get; set; }

        public int? WoCount { get; set; }

        public int? MatchesPlayed { get; set; }

        public int? MatchesWon { get; set; }

        public int? SetsWon { get; set; }

        public int? SetsLost { get; set; }

        public int? GamesWon { get; set; }

        public int? GamesLost { get; set; }
    }
}