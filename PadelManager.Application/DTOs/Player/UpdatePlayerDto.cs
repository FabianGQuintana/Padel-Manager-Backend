using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Player
{
    public class UpdatePlayerDto
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "DNI inválido.")]
        public string? Dni { get; set; }

        [Range(1, 120)]
        public Byte? Age { get; set; }

        public string? Availability { get; set; }
    }
}
