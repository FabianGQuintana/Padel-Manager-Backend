using System.Collections.Generic;
using System.Text;
using System;

namespace PadelManager.Application.DTOs.Registration
{
    public class RegistrationResponseDto
    {
        public Guid Id { get; set; }

        public DateOnly RegistrationDate { get; set; }

        public TimeOnly RegistrationTime { get; set; }

        public string IsActive { get; set; } = null!;

        public Guid CoupleId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TournamentId { get; set; }
        public string? CoupleNames { get; set; } 
        public string? CategoryName { get; set; }
        public string? TournamentName { get; set; }
    }
}
