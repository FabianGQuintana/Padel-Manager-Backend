using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Match : BaseEntity
    {
        public Guid? WinnerCoupleId { get; set; }
        public Guid? LoserCoupleId { get; set; }
        public DateTime DateTime { get; set; }
        public MatchStatus StatusType { get; set; } = MatchStatus.Pending;
        public string LocationName { get; set; } = string.Empty;
        public string CourtName { get; set; } = string.Empty;

        // Marcadores
        public int Set1_coupleA { get; set; }
        public int Set1_coupleB { get; set; }
        public int Set2_coupleA { get; set; }
        public int Set2_coupleB { get; set; }
        public int? Set3_coupleA { get; set; }
        public int? Set3_coupleB { get; set; }

        // --- Relaciones ---

        // Etapa (Obligatoria: Grupos, 4tos, etc.)
        public Guid StageId { get; set; }
        public Stage Stage { get; set; } = null!;

        // Zona (Opcional: Solo si es Fase de Grupos)
        public Guid? ZoneId { get; set; }
        public Zone? Zone { get; set; }

        // Pareja A (Obligatoria)
        public Guid CoupleId { get; set; }
        public Couple Couple { get; set; } = null!;

        // Pareja B (Obligatoria)
        public Guid CoupleId2 { get; set; }
        public Couple Couple2 { get; set; } = null!;
    }
}
