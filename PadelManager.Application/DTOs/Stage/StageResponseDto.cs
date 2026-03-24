using System;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Stage
{
    public class StageResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public StageType Type { get; set; }

        public int Order { get; set; }

        public Guid CategoryId { get; set; }

        // Datos calculados opcionales
        public int ZonesCount { get; set; }
        public int MatchesCount { get; set; }
    }
}
