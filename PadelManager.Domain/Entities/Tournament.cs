using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    

        //Relationships FK
        public Guid ManagerId { get; set; }
        
        //Navigation properties
        public  ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Manager> Managers { get; set; } = new List<Manager>();

        public bool CanChangeStatusToInProgress(IEnumerable<Category> categoriesWithCounts)
        {
            // El torneo solo puede empezar si está en borrador
            if (Status != TournamentStatus.Draft) return false;

            // Regla de negocio global: ¿Necesitamos que TODAS las categorías estén listas?
            // O quizás que al menos una tenga el mínimo para arrancar.
            return categoriesWithCounts.All(c => c.CanStartCategory(c.Registrations.Count));
        }

    }
}
