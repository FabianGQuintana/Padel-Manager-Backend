using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Registration
    {
        public required DateOnly RegistrationDate { get; set; }

        public required TimeOnly RegistrationTime { get; set; }

        //Relations FK
        public required Guid CoupleId { get; set; }

        public required Guid CategoryId { get; set; }

        //Relations Navigation

        public required Couple Couple { get; set; }

        public required Category Category { get; set; }

    }
}
