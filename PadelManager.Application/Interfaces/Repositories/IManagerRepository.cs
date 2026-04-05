using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        // Para traer los datos del perfil junto con los datos básicos del usuario
        Task<Manager?> GetManagerWithUserAsync(Guid managerId);

        Task<Manager?> GetManagerByUserIdAsync(Guid userId);

        Task<IEnumerable<Manager>> GetAllWithUsersAsync();
    }
}
