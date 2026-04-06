using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public required decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public required PaymentMethodTypes PaymentMethod { get; set; }
        public required PaymentStatusTypes PaymentStatus { get; set; } = PaymentStatusTypes.Pending;
        public required PaymentType Type { get; set; } // Es seña o saldo
        //FKs
        public Guid RegistrationId { get; set; }

        public Registration Registration { get; set; } = null!;
    }
}
