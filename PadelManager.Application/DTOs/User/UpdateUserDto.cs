using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
