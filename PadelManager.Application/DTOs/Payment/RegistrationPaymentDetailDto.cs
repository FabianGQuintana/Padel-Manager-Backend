using PadelManager.Application.DTOs.CoupleAvailability;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Payment
{
    public class RegistrationPaymentDetailDto
    {
        public int PairNumber { get; set; } // El "N° de Pareja" del PDF
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public string Zone { get; set; } = string.Empty;

        public decimal PriceToPay { get; set; } // TotalAmount de la Registration
        public decimal AmountPaid { get; set; } // Suma de sus pagos (Seña + Sede)
        public decimal DiscountApplied { get; set; } // Su descuento individual
        public decimal PendingBalance { get; set; } // Lo que debe esta pareja

        
        public List<CoupleAvailabilityResponseDto> ScheduleRestrictions { get; set; } = new();
    }
}
