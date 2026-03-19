using System;
using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Tournament
{
    public class CreateTournamentDto
    {

        public required string Name { get; set; } 

        public required DateTime StartDate { get; set; }

        public required string Regulations { get; set; }

        public required string TournamentType { get; set; } 

        public required Guid ManagerId { get; set; }

        [Range(6, 48)]
        public required int MaxTeamsPerCategory { get; set; }
    }
}
