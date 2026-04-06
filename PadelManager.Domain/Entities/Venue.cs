using System;
using PadelManager.Domain.Entities;

namespace PadelManager.Domain.Entities
{
    public class Venue : BaseEntity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; } 
        public string? PhoneNumber { get; set; }

        // Relación: Una sede tiene muchas canchas
        public  ICollection<Court> Courts { get; set; } = new List<Court>();
    }
}
