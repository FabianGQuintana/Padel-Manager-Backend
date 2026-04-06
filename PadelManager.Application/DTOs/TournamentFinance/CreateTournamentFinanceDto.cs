using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.TournamentFinance
{
    public class CreateTournamentFinanceDto
    {
        public string FinanceConcept { get; set; } = null!;
        public decimal Amount { get; set; }
        public TypeMovement MovementType { get; set; }
        public Guid TournamentId { get; set; }
    }
}
