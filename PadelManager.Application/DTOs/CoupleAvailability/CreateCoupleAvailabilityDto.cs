using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.CoupleAvailability
{
    public class CreateCoupleAvailabilityDto
    {
        public required DayOfWeek Day { get; set; } // Día de la semana (Lunes, Martes, etc.)
        public required TimeOnly From { get; set; } // Hora de inicio de la disponibilidad
        public required TimeOnly To { get; set; } // Hora de fin de la disponibilidad
        public required Guid CoupleId { get; set; }

    }
}
