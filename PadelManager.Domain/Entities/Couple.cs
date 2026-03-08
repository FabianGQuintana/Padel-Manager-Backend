using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Couple : BaseEntity
    {
        public required string Category { get; set; } // Categoría a la que pertenece la pareja (Veteranos, Libres, Menores, etc.)
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        // Relationships FK
        public Guid PlayerId { get; set; }
        // Navigation properties
        public required Player Player { get; set; }
        // Relationships FK
        public Guid ZoneId { get; set; }
        // Navigation properties
        public required Zone Zone { get; set; }
    }
}
