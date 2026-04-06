using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Court
{
    public class CreateCourtDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = null!;

        public string SurfaceType { get; set; } = "Sintético";

        [Required(ErrorMessage = "La disponibilidad de la cancha es obligatorio.")]
        public CourtAvailabilityType CourtAvailability { get; set; }

        [Required]
        public Guid VenueId { get; set; }
    }
}
