using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class CoupleAvailability : BaseEntity
    {
        public DayOfWeek Day { get; set; } // Día de la semana (Lunes, Martes, etc.)
        public TimeOnly From { get; set; } // Hora de inicio de la disponibilidad
        public TimeOnly To { get; set; } // Hora de fin de la disponibilidad    

        public Guid CoupleId { get; set; }
        public Couple Couple { get; set; } = null!;

    }
}
