using Microsoft.AspNetCore.Http;
using PadelManager.Application.Interfaces.Common;
using System.Security.Claims;

namespace PadelManager.Infrastructure.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Buscamos el claim "id" que es el que pusiste en el TokenService
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;

        public Guid Id => Guid.TryParse(UserId, out var guid) ? guid : Guid.Empty;

        
        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value
                                   ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}