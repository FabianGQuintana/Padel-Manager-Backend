using System;
using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Registration
{
    public class CreateRegistrationDto
    {
        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto total no puede ser negativo.")]
        public decimal TotalAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal Discount { get; set; } = 0; // Por defecto 0 si no mandan nada

        [Required(ErrorMessage = "La pareja es obligatoria.")]
        public Guid CoupleId { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "El torneo es obligatorio.")]
        public Guid TournamentId { get; set; }
    }
}