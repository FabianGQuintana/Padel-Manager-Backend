using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RoleName { get; set; } = null!; // "Organizador", "Invitado", etc.
    }
}
