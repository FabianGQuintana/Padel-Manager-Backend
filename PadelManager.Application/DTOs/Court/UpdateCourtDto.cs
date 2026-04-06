using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Court
{
    public class UpdateCourtDto
    {
        public string? Name { get; set; }

        public string? SurfaceType { get; set; }

        public CourtAvailabilityType? CourtAvailability { get; set; }
    }
}
