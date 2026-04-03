using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; } 
        public string Message { get; set; } = null!;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
    }
}
