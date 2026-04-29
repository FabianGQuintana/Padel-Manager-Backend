using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Tournament
{
    public class TournamentManagerSummaryDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
    }
}
