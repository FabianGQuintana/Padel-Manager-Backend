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
        public required Tournament Tournament { get; set; }
    }
}
