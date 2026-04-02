using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Manager : BaseEntity
    {
        public byte? YearExperience {  get; set; }

        public string? LicenceAPA { get; set; }

        //Fk
        public Guid UserId { get; set; }
        
        public User User { get; set; } = null!;

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}

