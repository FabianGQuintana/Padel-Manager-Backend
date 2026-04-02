using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class LoginUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}