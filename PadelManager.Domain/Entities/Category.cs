using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Relación obligatoria por FK (Un torneo es dueño de la categoría)
        public Guid TournamentId { get; set; }

        public Tournament? Tournament { get; set; }

        public ICollection<Stage> Stages { get; set; } = new List<Stage>();

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
