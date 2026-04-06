using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Court
{
    public class CourtResponseDto
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }
        public string Name { get; set; } = null!;
        public string SurfaceType { get; set; } = null!;
        public CourtAvailabilityType CourtAvailability { get; set; }

    }
}
