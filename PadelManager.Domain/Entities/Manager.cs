using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Manager : BaseEntity
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required int Dni { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
}
