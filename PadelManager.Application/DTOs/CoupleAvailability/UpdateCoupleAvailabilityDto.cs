using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.CoupleAvailability
{
    public class UpdateCoupleAvailabilityDto
    {
        public DayOfWeek? Day { get; set; } // Día de la semana (Lunes, Martes, etc.)
        public TimeOnly? From { get; set; } // Hora de inicio de la disponibilidad
        public TimeOnly? To { get; set; } // Hora de fin de la disponibilidad
    }
}
