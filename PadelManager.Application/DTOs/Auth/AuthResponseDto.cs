using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Name { get; set; } = null!;
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public Guid UserId { get; set; }
    }
}
