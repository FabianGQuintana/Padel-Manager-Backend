using System;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    //Esta clase servirá como base para todos los repositorios específicos
    //de cada entidad (Player, Match, etc.) Donde definiremos los métodos
    //comunes a todos ellos (GetById, GetAll, Add, Update, Delete, etc.)
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id); // Obtener una entidad por su ID

        Task<IEnumerable<T>> GetAllAsync(); //Podemos realizar un paginado por FRONTEND

        Task<T> AddAsync(T entity); // Agregar una nueva entidad

        Task<T> UpdateAsync(T entity); // Actualizar una entidad existente
         
        Task<T?> SoftDeleteToggleAsync(Guid id); // Eliminar una entidad por su ID

    }
}
