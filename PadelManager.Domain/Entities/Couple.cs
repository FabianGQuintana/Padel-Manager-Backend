using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Couple : BaseEntity
    {
      
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)

        // Jugador 1
        public Guid Player1Id { get; set; }
        public Player Player1 { get; set; } = null!;

        // Jugador 2
        public Guid Player2Id { get; set; }
        public Player Player2 { get; set; } = null!;


       public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
       public ICollection<CoupleAvailability> Availabilities { get; set; } = new List<CoupleAvailability>();
    }
}
