using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.CoupleAvailability
{
    public class CoupleAvailabilityResponseDto
    {
        public Guid Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }
        public Guid CoupleId { get; set; }
    }
}
