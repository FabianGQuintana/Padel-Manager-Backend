using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class UpdateManagerDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public byte? YearExperience { get; set; }
        public string? LicenceAPA { get; set; }
    }
}
