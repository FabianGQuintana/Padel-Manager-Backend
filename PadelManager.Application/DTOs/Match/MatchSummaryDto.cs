using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
namespace PadelManager.Application.DTOs.Match
{
    public class MatchSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public MatchStatus Status { get; set; }

        // Se manda el nombre armado del enfrentamiento para que el front no tenga que hacer trabajo extra
        // Imaginense la pantalla principal del torneo, donde solo se muestre la agenda del día
        // Ej: "Pérez/Gómez vs. Díaz/López"
        public string MatchTitle { get; set; } = string.Empty;
    }
}
