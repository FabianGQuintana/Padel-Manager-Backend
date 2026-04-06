using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.TournamentFinance
{
    public class UpdateTournamentFinanceDto
    {
        public string? FinanceConcept { get; set; }
        public decimal? Amount { get; set; }
        public TypeMovement? MovementType { get; set; }
    }
}
