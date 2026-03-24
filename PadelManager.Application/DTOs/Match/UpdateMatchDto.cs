using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Match
{
    public class UpdateMatchDto
    {
        public Guid? WinnerCoupleId { get; set; }
        public Guid? LoserCoupleId { get; set; }
        public DateTime? DateTime { get; set; }
        public MatchStatus? Status { get; set; }
        public string? LocationName { get; set; }
        public string? CourtName { get; set; }

        // Scoreboard
        public int? Set1_coupleA { get; set; }
        public int? Set1_coupleB { get; set; }
        public int? Set2_coupleA { get; set; }
        public int? Set2_coupleB { get; set; }
        public int? Set3_coupleA { get; set; }
        public int? Set3_coupleB { get; set; }
    }
}
