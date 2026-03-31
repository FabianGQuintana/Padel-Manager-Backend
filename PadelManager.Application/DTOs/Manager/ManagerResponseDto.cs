using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class ManagerResponseDto
    {
        public Guid Id { get; set; } // Recordá que es el mismo que UserId
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public byte? YearExperience { get; set; }
        public string? LicenceAPA { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
