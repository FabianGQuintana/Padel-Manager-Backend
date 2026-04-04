using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Auth;
using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Services;
using System.Security.Authentication;

namespace PadelManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public AuthController(IAuthService authService, IUserService userService, ICurrentUser currentUser)
        {
            _authService = authService;
            _userService = userService;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterManagerDto dto)
        {
            var result = await _authService.RegisterManagerAsync(dto);
             
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Redirigimos a la consulta del usuario recién creado
            return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Sesión inválida o expirada." });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await _authService.LogoutAsync(request.RefreshToken);
            return Ok(new { message = "Sesión cerrada correctamente." });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            //  Usamos directamente ICurrentUser para obtener el ID de forma limpia
            if (!_currentUser.IsAuthenticated)
                return Unauthorized();

            var result = await _authService.ChangePasswordAsync(_currentUser.Id, dto);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = "Contraseña actualizada con éxito." });
        }

        [Authorize]
        [HttpGet("me/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado." });

            return Ok(user);
        }
    }
}