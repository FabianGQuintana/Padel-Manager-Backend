using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class TournamentFinance : BaseEntity
    {
        public required string FinanceConcept {  get; set; }

        public required decimal Amount { get; set; }
        public required TypeMovement MovementType { get; set; }
        public DateTime DateMovement { get; set; } = DateTime.UtcNow;

        //FK

        public Guid TournamentId { get; set; }

        public Tournament Tournament { get; set; } = null!;
    }
}
