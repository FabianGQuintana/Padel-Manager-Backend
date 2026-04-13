using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class UpdateManagerDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\+?(\d{10,15})$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos.")]
        public string PhoneNumber { get; set; } = null!;

        [Range(0, 60, ErrorMessage = "Los años de experiencia deben estar entre 0 y 60.")]
        public byte? YearExperience { get; set; }

        [StringLength(20, ErrorMessage = "La licencia APA no puede superar los 20 caracteres.")]
        public string? LicenceAPA { get; set; }
    }
}
