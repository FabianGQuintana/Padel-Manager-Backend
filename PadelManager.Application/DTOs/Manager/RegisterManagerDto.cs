using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class RegisterManagerDto
    {
        // Datos de User
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Dni { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid RoleId { get; set; }

        // Datos de Manager
        public byte? YearExperience { get; set; }
        public string? LicenceAPA { get; set; }
    }
}
