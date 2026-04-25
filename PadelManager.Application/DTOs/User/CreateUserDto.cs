using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "El email debe tener un formato válido (ejemplo@dominio.com).")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio")]
        public Guid RoleId { get; set; }
    }
}
