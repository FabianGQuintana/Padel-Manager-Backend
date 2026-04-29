using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Tournament
{
    public class UpdateTournamentDto
    {

        public string? Name { get; set; }

        public string? Regulations { get; set; }

        public string? TournamentType { get; set; }

        public DateTime? StartDate { get; set; }

        public List<Guid>? ManagerIds { get; set; } = new List<Guid>();

        public  int? MaxTeamsPerCategory { get; set; }


    }
}
