using System;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByDniAsync(string dni);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name);
        Task<IEnumerable<User>> GetUsersByLastNameAsync(string name);
        Task<IEnumerable<User>> GetUsersByRoleNameAsync(string roleName);
        Task AddRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task UpdateRefreshTokenAsync(RefreshToken token);
        Task<User?> GetUserByIdWithRoleAsync(Guid id);
        // Método para traer al usuario con su perfil de Manager 
        Task<User?> GetUserWithManagerProfileAsync(Guid userId);
    }
}
