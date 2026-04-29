using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Player
{
    public class CreatePlayerDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\+?(\d{10,15})$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos.")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "DNI inválido.")]
        public required string Dni { get; set; }

        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120 años.")]
        public Byte? Age { get; set; }

        [StringLength(100, ErrorMessage = "La disponibilidad no puede superar los 100 caracteres.")]
        public string? Availability { get; set; }
    }
}
