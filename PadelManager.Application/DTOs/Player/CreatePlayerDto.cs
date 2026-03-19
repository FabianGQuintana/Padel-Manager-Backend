using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Player
{
    public class CreatePlayerDto
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "DNI inválido.")]
        public required string Dni { get; set; }

        [Range(1, 120)]
        public Byte? Age { get; set; }

        public required string Availability { get; set; }
    }
}
