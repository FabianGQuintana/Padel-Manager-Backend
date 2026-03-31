using PadelManager.Application.DTOs.Auth;
using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Common; 
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IManagerRepository _managerRepository; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher; 

        public AuthService(
            IUserRepository userRepository,
            IManagerRepository managerRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IPasswordHasher passwordHasher) 
        {
            _userRepository = userRepository;
            _managerRepository = managerRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        #region REGISTRO Y LOGIN

        public async Task<AuthResponseDto> RegisterManagerAsync(RegisterManagerDto dto)
        {
            if (await _userRepository.GetUserByEmailAsync(dto.Email) != null)
                return new AuthResponseDto { Success = false, Message = "El email ya está registrado." };

            if (await _userRepository.GetUserByDniAsync(dto.Dni) != null)
                return new AuthResponseDto { Success = false, Message = "El DNI ya está registrado." };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                LastName = dto.LastName,
                Dni = dto.Dni,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password), //  Usamos el hasher
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            var managerProfile = new Manager
            {
                Id = user.Id,
                UserId = user.Id,
                YearExperience = dto.YearExperience,
                LicenceAPA = dto.LicenceAPA,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _userRepository.AddAsync(user);
            await _managerRepository.AddAsync(managerProfile); //  FIX: Adiós al error del Set<Manager>

            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto { Success = true, Message = "Registro exitoso.", UserId = user.Id };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            // FIX: Verificamos con la interfaz
            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                return new AuthResponseDto { Success = false, Message = "Credenciales incorrectas." };
            }

            var accessToken = _tokenService.GenerateToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddRefreshTokenAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                Expiration = DateTime.UtcNow.AddHours(8),
                UserId = user.Id,
                Name = user.Name
            };
        }

        #endregion

        #region GESTIÓN DE TOKENS

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            // 1. Buscamos el token en la base de datos
            var storedToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);

            // Validamos que exista, que no haya sido usado y que no esté revocado/vencido
            if (storedToken == null || !storedToken.IsActive)
                throw new InvalidOperationException("Token de refresco inválido, vencido o ya utilizado.");

            // 2. Extraemos el Principal del Access Token (aunque esté vencido)
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            //  MEJORA: Buscamos el claim de forma segura (tolerando "id" o el estándar "sub")
            var userIdClaim = principal.Claims.FirstOrDefault(c =>
            c.Type == "id" || c.Type == "sub");

            if (userIdClaim == null)
                throw new InvalidOperationException("El token de acceso no contiene una identidad válida.");

            var userId = Guid.Parse(userIdClaim.Value);

            // 3. Obtenemos el usuario con su perfil
            var user = await _userRepository.GetUserWithManagerProfileAsync(userId);

            // Verificamos que el usuario exista y que el token realmente le pertenezca a él
            if (user == null || storedToken.UserId != user.Id)
                throw new InvalidOperationException("Intento de refresco no autorizado o inconsistente.");

            // 4.ROTACIÓN DE TOKEN (Seguridad Versori 2)
            // Marcamos el token viejo como usado para que no se pueda reutilizar
            storedToken.IsUsed = true;
            await _userRepository.UpdateRefreshTokenAsync(storedToken);

            // 5. Generamos el nuevo par de llaves
            var newAccessToken = _tokenService.GenerateToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();

            // Guardamos el nuevo Refresh Token en la DB
            await _userRepository.AddRefreshTokenAsync(new RefreshToken
            {
                Token = newRefreshTokenValue,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7), // Mantenemos la sesión viva una semana más
                CreatedAt = DateTime.UtcNow
            });

            // 6. Impactamos todos los cambios en una sola transacción
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue,
                UserId = user.Id,
                Name = user.Name, // Agregamos el nombre para que el Front no lo pierda al refrescar
                Expiration = DateTime.UtcNow.AddHours(8) // Sincronizamos la expiración
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                await _userRepository.UpdateRefreshTokenAsync(storedToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return (false, "Usuario no encontrado.");

            if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
                return (false, "La contraseña actual es incorrecta.");

            user.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return (true, null);
        }

        #endregion
    }
}