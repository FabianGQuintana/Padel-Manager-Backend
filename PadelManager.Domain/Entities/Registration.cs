using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Registration : BaseEntity
    {
        public required DateOnly RegistrationDate { get; set; }

        public required TimeOnly RegistrationTime { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }

        //Relations FK
        public  Guid CoupleId { get; set; }

        public  Guid CategoryId { get; set; }

        public Guid TournamentId { get; set; }

        //Relations Navigation

        public Couple Couple { get; set; } = null!;

        public  Category Category { get; set; } = null!;

        public Tournament Tournament { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
