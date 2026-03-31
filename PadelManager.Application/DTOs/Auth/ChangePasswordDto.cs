using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
