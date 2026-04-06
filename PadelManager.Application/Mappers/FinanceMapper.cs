using PadelManager.Application.DTOs.CoupleAvailability;
using PadelManager.Application.DTOs.Payment;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System.Linq;

namespace PadelManager.Application.Mappers
{
    public static class FinanceMapper
    {
        // 1. Mapeo del Cuadro Resumen Azul (TournamentFinancialSummaryDto)
        public static TournamentFinancialSummaryDto ToFinancialSummaryDto(this Tournament tournament)
        {
            var registrations = tournament.Registrations;

            // Suma de todas las señas aprobadas
            decimal deposits = registrations.SelectMany(r => r.Payments)
                .Where(p => p.Type == PaymentType.Deposit && p.PaymentStatus == PaymentStatusTypes.Approved)
                .Sum(p => p.Amount);

            // Suma de todos los cobros en sede aprobados
            decimal onSite = registrations.SelectMany(r => r.Payments)
                .Where(p => p.Type == PaymentType.FinalBalance && p.PaymentStatus == PaymentStatusTypes.Approved)
                .Sum(p => p.Amount);

            decimal expected = registrations.Sum(r => r.TotalAmount);
            decimal discounts = registrations.Sum(r => r.Discount);

            return new TournamentFinancialSummaryDto
            {
                TournamentName = tournament.Name,
                TotalExpected = expected,
                TotalDeposits = deposits,
                TotalDiscounts = discounts,
                TotalOnSiteCollected = onSite,
                // Cálculo automático: Lo que falta entrar a caja
                TotalDebt = expected - (deposits + onSite + discounts),
                FinalTotalCollected = deposits + onSite
            };
        }

        // 2. Mapeo de la Pareja Individual para la Grilla
        public static RegistrationPaymentDetailDto ToPaymentDetailDto(this Registration reg, int index)
        {
            // Sumamos todos sus pagos aprobados (seña + sede)
            decimal totalPaid = reg.Payments
                .Where(p => p.PaymentStatus == PaymentStatusTypes.Approved)
                .Sum(p => p.Amount);

            return new RegistrationPaymentDetailDto
            {
                PairNumber = index,
                Player1Name = $"{reg.Couple.Player1.Name} {reg.Couple.Player1.LastName}",
                Player2Name = $"{reg.Couple.Player2.Name} {reg.Couple.Player2.LastName}",
                PriceToPay = reg.TotalAmount,
                AmountPaid = totalPaid,
                DiscountApplied = reg.Discount,
                // Si el resultado es 0, aparece como "Pagado" en el PDF
                PendingBalance = reg.TotalAmount - totalPaid - reg.Discount,

                // Opción B: Pasamos la lista de objetos de disponibilidad cruda
                ScheduleRestrictions = reg.Couple.Availabilities.Select(a => new CoupleAvailabilityResponseDto
                {
                    Id = a.Id,
                    Day = a.Day,
                    From = a.From,
                    To = a.To,
                    CoupleId = a.CoupleId,
                    IsActive = a.DeletedAt == null ? "Activo" : "Inactivo"
                }).ToList()
            };
        }

        public static List<CategoryPaymentGridDto> ToCategoryGrids(this Tournament tournament)
        {
            return tournament.Categories.Select(cat => new CategoryPaymentGridDto
            {
                CategoryName = cat.Name,
                PairDetails = tournament.Registrations
                    .Where(r => r.CategoryId == cat.Id)
                    .Select((r, index) => r.ToPaymentDetailDto(index + 1))
                    .ToList(),
                CategoryTotalExpected = tournament.Registrations
                    .Where(r => r.CategoryId == cat.Id).Sum(r => r.TotalAmount),
                CategoryTotalCollected = tournament.Registrations
                    .Where(r => r.CategoryId == cat.Id)
                    .SelectMany(r => r.Payments)
                    .Where(p => p.PaymentStatus == PaymentStatusTypes.Approved)
                    .Sum(p => p.Amount)
            }).ToList();
        }


    }
}
