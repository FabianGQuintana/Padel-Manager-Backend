using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Venue
{
    public class UpdateVenueDto
    {
        public string? Name { get; set; } 
        public string? Address { get; set; } 
        public string? City { get; set; } 
        public string? PhoneNumber { get; set; }
    }
}
