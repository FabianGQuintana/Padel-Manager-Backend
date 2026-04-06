using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Player : BaseEntity
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Dni { get; set; }

        public Byte? Age { get; set; }

        public string? Availability { get; set; }

        public ICollection<Sanction> Sanctions { get; set; } = new List<Sanction>();

    }
}
