using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Application.DTOs.CoupleAvailability;


namespace PadelManager.Application.DTOs.Couple
{
    public class CreateCoupleDto
    {
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public List<CreateCoupleAvailabilityDto> Availabilities { get; set; } = new();

    }
}
