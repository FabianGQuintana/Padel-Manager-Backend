using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class CreateManagerDto
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required string Dni { get; set; } 

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }
    }
}
