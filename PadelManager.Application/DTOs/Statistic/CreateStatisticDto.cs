using System;
using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Statistic
{
    public class CreateStatisticDto
    {
        [Required] 
        public required Guid CoupleId { get; set; }

        [Required]
        public required Guid ZoneId { get; set; }

        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        [Range(0, int.MaxValue)]
        public int WoCount { get; set; }

        [Range(0, int.MaxValue)]
        public int MatchesPlayed { get; set; }

        [Range(0, int.MaxValue)]
        public int MatchesWon { get; set; }

        [Range(0, int.MaxValue)]
        public int SetsWon { get; set; }

        [Range(0, int.MaxValue)]
        public int SetsLost { get; set; }

        [Range(0, int.MaxValue)]
        public int GamesWon { get; set; }

        [Range(0, int.MaxValue)]
        public int GamesLost { get; set; }
    }
}