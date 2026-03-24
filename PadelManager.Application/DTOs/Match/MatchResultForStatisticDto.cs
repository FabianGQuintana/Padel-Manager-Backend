using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Match
{
    public class MatchResultForStatisticDto
    {
        public Guid MatchId { get; set; } // Para saber de qué partido viene la info


        // juagadores involucrados (para sumar/restar puntos)
        public Guid WinnerCoupleId { get; set; }
        public Guid LoserCoupleId { get; set; }


        // --- Datos para los desempates en las tablas de posiciones

        // para calcular la "Diferencia de Sets" y "Diferencia de Juegos".
        public int SetsWonByWinner { get; set; }
        public int SetsWonByLoser { get; set; }

        public int TotalGamesWonByWinner { get; set; }
        public int TotalGamesWonByLoser { get; set; }

        // Instancia donde se aplican los puntos (No es lo mismo un partido de Zona que una Eliminatoria)
        public Guid StageId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
