using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Stage : BaseEntity
    {
        //  Aquí va "Octavos", "Cuartos", "Fase de Grupos", etc.
        public required string Name { get; set; }

        // Aquí definimos si es grupo o eliminación
        public StageType Type { get; set; }

        //  1 para Grupos, 2 para 8vos, 3 para 4tos...
        public int Order { get; set; }

        // Relaciones
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Colecciones (Navigation Properties)
        public ICollection<Zone> Zones { get; set; } = new List<Zone>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
