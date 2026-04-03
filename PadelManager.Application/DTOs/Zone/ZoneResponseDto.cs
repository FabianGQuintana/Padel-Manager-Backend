using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Application.DTOs.Match;
using PadelManager.Application.DTOs.Couple;
using PadelManager.Application.DTOs.Statistic;

namespace PadelManager.Application.DTOs.Zone
{
    public class ZoneResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid StageId { get; set; }
        public string IsActive { get; set; } = null!;

        // Relaciones transformadas a DTOs
        public List<CoupleResponseDto> Couples { get; set; } = new();
        public List<MatchResponseDto> Matches { get; set; } = new();

        // Opcional
        public List<StatisticResponseDto> Statistics { get; set; } = new();

    }
}
