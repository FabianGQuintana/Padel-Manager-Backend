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

        //Relationships FK
        public Guid ManagerId { get; set; }
        
        //Navigation properties
        public required Manager Manager { get; set; }

    }
}
