using PadelManager.Application.DTOs.Auth;
using PadelManager.Application.DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IAuthService
    {
        // Registro específico para Managers (Crea User + Perfil Manager)
        Task<RegisterResponseDto> RegisterManagerAsync(RegisterManagerDto dto);

        // Login tradicional
        Task<AuthResponseDto> LoginAsync(LoginUserDto dto);

        // Renovación de sesión sin pedir contraseña
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        // Cierre de sesión (Revoca el Refresh Token)
        Task LogoutAsync(string refreshToken);

        // Cambio de contraseña
        Task<(bool Success, string? ErrorMessage)> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    }
}
