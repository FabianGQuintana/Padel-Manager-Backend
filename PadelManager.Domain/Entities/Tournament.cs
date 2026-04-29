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

        public required TournamentStatus StatusType { get; set; } = TournamentStatus.Draft;

        public required string TournamentType { get; set; } //Para diferenciar si es "Veteranos", "Libres" o "Menores".

        
        //Navigation properties
        public  ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Manager> Managers { get; set; } = new List<Manager>();

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

       

    }
}
