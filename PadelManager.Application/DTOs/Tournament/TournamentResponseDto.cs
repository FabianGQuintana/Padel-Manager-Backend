using System;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Tournament
{
    public class TournamentResponseDto
    {
        public  Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public  DateTime StartDate { get; set; }

        public  string? Regulations { get; set; } 

        public  string Status { get; set; } = null!;

        public string IsActive { get; set; } = null!;

        public string TournamentType { get; set; } = null!;

        public Guid ManagerId { get; set; }

        public string ManagerName { get; set; } = null!;


    }
}
