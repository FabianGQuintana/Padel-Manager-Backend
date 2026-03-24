using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Range(6, 48, ErrorMessage = "La categoría debe tener entre 6 y 48 parejas.")]
        public int MaxTeams { get; set; } = 48;

        // Relación obligatoria por FK (Un torneo es dueño de la categoría)
        public Guid TournamentId { get; set; }

        public Tournament? Tournament { get; set; }

        public ICollection<Stage> Stages { get; set; } = new List<Stage>();

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        public bool IsFull(int registrationCount) => registrationCount >= MaxTeams;

        public bool CanStart(int registrationCount) => registrationCount >= 6 && registrationCount <= MaxTeams;

        public bool CanStartCategory(int currentRegistrationCount)
        {
            // La regla de negocio ahora es propia de la categoría
            if (currentRegistrationCount < 6 || currentRegistrationCount > MaxTeams)
            {
                return false;
            }

            return true;
        }

    }
}
