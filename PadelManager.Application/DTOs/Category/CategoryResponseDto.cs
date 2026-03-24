using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Category
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int MaxTeams { get; set; }
        public Guid TournamentId { get; set; }

        // Datos calculados para React/Vite
        public int RegistrationCount { get; set; }
        public bool IsFull { get; set; }
        public bool CanStart { get; set; }

    }
}
