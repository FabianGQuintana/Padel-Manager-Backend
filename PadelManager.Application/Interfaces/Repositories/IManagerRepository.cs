using System;
using PadelManager.Domain.Entities;


namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        Task<Manager?> GetManagerByNameAsync(string nameManager); // Método específico para obtener un manager por su nombre

        Task<Manager?> GetManagerByDNIAsync(int dniManager); // Método específico para obtener un manager por su DNI

        Task<Manager?> GetManagerByEmailAsync(string emailManager); // Método específico para obtener un manager por su Email

        Task<Manager?> GetManagerByPhoneAsync(string phoneManager); // Método específico para obtener un manager por su teléfono

        //Aca podemos seguir agregando mas metodos especificos para manager en un futuro ej: obtener por lista de manager. 
    }
}
