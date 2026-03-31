using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
