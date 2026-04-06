using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Venue
{
    public class VenueResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string IsActive { get; set; } = null!;
    }    
}
