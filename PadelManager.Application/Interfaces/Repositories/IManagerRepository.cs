using PadelManager.Application.DTOs.Manager;
using PadelManager.Domain.Entities;
using System;


namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        Task<IEnumerable<Manager>> GetManagersByNameAsync(string nameManager); // Método específico para obtener un manager por su nombre

        Task<IEnumerable<Manager>> GetManagersByLastNameAsync(string lastName);

        Task<Manager?> GetManagerByDNIAsync(string dniManager); // Método específico para obtener un manager por su DNI

        Task<Manager?> GetManagerByEmailAsync(string emailManager); // Método específico para obtener un manager por su Email

        Task<Manager?> GetManagerByPhoneAsync(string phoneManager); // Método específico para obtener un manager por su teléfono

        Task<Manager?> GetManagerWithTournamentsAsync(Guid managerId);
    }
}
