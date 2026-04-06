using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Venue
{
    public class CreateVenueDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;

    }
}
