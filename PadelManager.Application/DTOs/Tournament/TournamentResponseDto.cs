using System;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Tournament
{
    public class TournamentResponseDto
    {
        public  Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public  DateTime StartDate { get; set; }

        public  string? Regulations { get; set; } 

        public  TournamentStatus Status { get; set; }

        public string TournamentType { get; set; } = null!;

        public int MaxTeamsPerCategory { get; set; } // Para mostrar una posible barra de la cantidad de inscriptos. ej.

        public Guid ManagerId { get; set; }

        public bool IsFull { get; set; }
        public bool IsReadyToStart { get; set; }
        public bool HasIdealZoneCount { get; set; }
    }
}
