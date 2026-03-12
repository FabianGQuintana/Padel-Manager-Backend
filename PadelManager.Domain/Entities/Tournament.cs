using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public required string Name { get; set; }

        public required DateTime StartDate { get; set; }

        public required string Regulations { get; set; }

        public required TournamentStatus Status { get; set; } = TournamentStatus.Draft;

        public required string TournamentType { get; set; } //Para diferenciar si es "Veteranos", "Libres" o "Menores".

        public required int MaxTeamsPerCategory { get; set; } // Límite de parejas por categoría para cerrar inscripciones automáticamente.

        //Relationships FK
        public Guid ManagerId { get; set; }
        
        //Navigation properties
        public  ICollection<Category> Categories { get; set; } = new List<Category>();
        
    }
}
