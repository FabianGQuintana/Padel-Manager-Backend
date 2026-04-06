using PadelManager.Domain.Enum;

namespace PadelManager.Domain.Entities
{
    public class Court : BaseEntity
    {
        public required string Name { get; set; }
        public required string SurfaceType { get; set; } // Ej: Sintético, Cemento
        public CourtAvailabilityType CourtAvailability { get; set; }

        // Clave Foránea a Venue
        public Guid VenueId { get; set; }
        public  Venue Venue { get; set; } = null!;
    }
}
