using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Court : BaseEntity
    {
        public required string Name { get; set; }
        public string? SurfaceType { get; set; } // Ej: Sintético, Cemento

        // Clave Foránea a Venue
        public Guid VenueId { get; set; }
        public  Venue Venue { get; set; } = null!;
    }
}
