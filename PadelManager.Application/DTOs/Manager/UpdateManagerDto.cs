using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class UpdateManagerDto
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Dni {  get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
