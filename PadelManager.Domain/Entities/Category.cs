using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; } 

        public  string? Description { get; set; }

        //Relationships FK
        public Guid TournamentId { get; set; }

        //Navigation properties
        public virtual required Tournament Tournament { get; set; }

        public virtual required ICollection<Stage> Stages { get; set; } = new List<Stage>();

        public   ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
