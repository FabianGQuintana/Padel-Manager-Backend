using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class LogoutRequestDto {
        public string RefreshToken { get; set; } = null!;
    }
}
