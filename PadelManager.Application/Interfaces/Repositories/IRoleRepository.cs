using PadelManager.Domain.Entities;
using System;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}
