using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Player
{
    public class UpdatePlayerDto
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string? Name { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public string? LastName { get; set; }

        [RegularExpression(@"^\+?(\d{10,15})$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos.")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "DNI inválido.")]
        public string? Dni { get; set; }

        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120 años.")]
        public Byte? Age { get; set; }

        [StringLength(100, ErrorMessage = "La disponibilidad no puede superar los 100 caracteres.")]
        public string? Availability { get; set; }
    }
}
