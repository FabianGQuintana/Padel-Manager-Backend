using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.TournamentFinance
{
    public class TournamentFinanceResponseDto
    {
        public Guid Id { get; set; }
        public string FinanceConcept { get; set; } = null!;
        public decimal Amount { get; set; }
        public string MovementType { get; set; } = null!; // "Income" o "Expense"
        public DateTime DateMovement { get; set; }
        public Guid TournamentId { get; set; }
    }
}
