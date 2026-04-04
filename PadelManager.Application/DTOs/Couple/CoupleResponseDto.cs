using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Application.DTOs.CoupleAvailability;

namespace PadelManager.Application.DTOs.Couple
{
    public class CoupleResponseDto
    {
        public Guid Id { get; set; }
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        public Guid Player1Id { get; set; }
        public string Player1Name { get; set; } = null!;

        public Guid Player2Id { get; set; }
        public string Player2Name { get; set; } = null!;

        public string IsActive { get; set; } = null!;

        public Guid? ZoneId { get; set; }
         public string? ZoneName { get; set; }
         public List<CoupleAvailabilityResponseDto> Availabilities { get; set; } = new();
    }
}
