using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string Type { get; set; } = null!; // "Deposit" o "FinalBalance"
        public Guid RegistrationId { get; set; }
    }
}
