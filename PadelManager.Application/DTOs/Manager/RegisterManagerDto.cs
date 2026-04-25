using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class RegisterManagerDto
    {
        // Datos de User
        // 1. Nombre y Apellido: Evitamos strings vacíos o nombres de una sola letra
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public required string LastName { get; set; }

        // 2. Email: Validación automática de formato (que tenga @, punto, etc.)
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "El email debe tener un formato válido (ejemplo@dominio.com).")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos numéricos.")]
        public required string Dni { get; set; }

        //  Aunque existe [Phone], un Regex es mejor para Argentina
        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\+?(\d{10,15})$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos.")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public Guid RoleId { get; set; }


        //Datos del manager.
        [Range(0, 60, ErrorMessage = "Los años de experiencia deben estar entre 0 y 60.")]
        public byte? YearExperience { get; set; }

        [StringLength(20, ErrorMessage = "La licencia APA no puede superar los 20 caracteres.")]
        public string? LicenceAPA { get; set; }
    }
}
