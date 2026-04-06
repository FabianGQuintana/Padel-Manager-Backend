using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Payment
{
    public class CategoryPaymentGridDto
    {
        public string CategoryName { get; set; } = string.Empty; // Ej: "Caballeros 7ma"
        public List<RegistrationPaymentDetailDto> PairDetails { get; set; } = new();

        // Totales parciales por categoría para facilitar el control
        public decimal CategoryTotalExpected { get; set; }
        public decimal CategoryTotalCollected { get; set; }
    }
}
