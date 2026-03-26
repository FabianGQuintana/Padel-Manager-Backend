using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Application.DTOs.CoupleAvailability;

namespace PadelManager.Application.DTOs.Couple
{
    public class UpdateCoupleDto
    {
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public List<UpdateCoupleAvailabilityDto> Availabilities { get; set; } = new();
    }
}
