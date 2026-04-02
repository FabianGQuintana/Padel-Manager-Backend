using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        // Útil para buscar el ID del rol por su nombre en el Enum
        Task<Role?> GetByNameAsync(TypeUser roleName);
    }
}
