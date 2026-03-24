using System;
using System.Collections.Generic;
using PadelManager.Domain.Enum;
using PadelManager.Application.DTOs.Match; // Asumiendo que tenés acceso al DTO de Match

namespace PadelManager.Application.DTOs.Stage
{
    public class StageBracketDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public StageType Type { get; set; }
        public int Order { get; set; }


        // Esta dto es para poder ver los partidos que hay adentro de X etapa    
        public IEnumerable<MatchResponseDto> Matches { get; set; } = new List<MatchResponseDto>();
    }
}
