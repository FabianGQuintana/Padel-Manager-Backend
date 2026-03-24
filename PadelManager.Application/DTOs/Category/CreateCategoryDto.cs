using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Range(6, 48, ErrorMessage = "La categoría debe tener entre 6 y 48 parejas.")]
        public int MaxTeams { get; set; } = 48;

        [Required]
        public Guid TournamentId { get; set; }
    }
}
