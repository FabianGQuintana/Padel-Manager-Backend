using PadelManager.Domain.Entities;
using System.Security.Claims;

namespace PadelManager.Application.Interfaces.Common
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
