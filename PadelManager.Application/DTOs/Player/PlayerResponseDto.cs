using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Player
{
    public class PlayerResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Dni { get; set; } = null!;

        public Byte? Age { get; set; }

        public string Availability { get; set; } = null!;
    }
}
