using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Payment
{
    public class TournamentFinancialSummaryDto
    {
        public string TournamentName { get; set; } = string.Empty;

        // 1. Total que debería entrar si todos pagaran el 100%
        public decimal TotalExpected { get; set; }

        // 2. Suma de todos los pagos marcados como "Deposit" (Señas)
        public decimal TotalDeposits { get; set; }

        // 3. Suma de todos los campos "Discount" de las inscripciones
        public decimal TotalDiscounts { get; set; }

        // 4. Suma de todos los pagos marcados como "FinalBalance" (En Sede)
        public decimal TotalOnSiteCollected { get; set; }

        // 5. Lo que falta cobrar (Calculado: Expected - Deposits - Discounts - OnSite)
        public decimal TotalDebt { get; set; }

        // 6. El neto real (Deposits + OnSite)
        public decimal FinalTotalCollected { get; set; }
    }
}
