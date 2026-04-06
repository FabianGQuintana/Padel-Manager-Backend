using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public PaymentMethodTypes PaymentMethod { get; set; }
        public PaymentType Type { get; set; } // Seña o Saldo
        public Guid RegistrationId { get; set; }
    }
}
