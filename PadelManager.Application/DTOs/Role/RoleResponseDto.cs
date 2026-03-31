using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Role
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // Aquí mandaremos el string del Enum
    }
}
