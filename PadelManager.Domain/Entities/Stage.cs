using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Stage : BaseEntity
    {
        public required string Group { get; set; } // Grupo A, B, C, etc.
        public required string Deletion { get; set; }
        public required string Order { get; set; } // Para ordenar las fases (1, 2, 3, etc.)

        // Relationships FK
        public Guid CategoryId { get; set; }


        // Navigation properties
        public required Category Category { get; set; }

        public   ICollection<Instance> Instances { get; set; } = new List<Instance>();

        public   ICollection<Zone> Zones { get; set; } = new List<Zone>();


    }
}
