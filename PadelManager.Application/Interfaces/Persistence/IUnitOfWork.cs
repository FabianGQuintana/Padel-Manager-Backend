using System;
using PadelManager.Domain.Entities;
using PadelManager.Application.Interfaces.Repositories;

namespace PadelManager.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        // para cada entidad que tengamos, añadiremos un repositorio
        IGenericRepository<Manager> Managers { get; }
        IGenericRepository<Player> Players { get; }
        IGenericRepository<Couple> Couples { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Stage> Stages { get; }
        IGenericRepository<Tournament> Tournaments { get; }
        IGenericRepository<Registration> Registrations { get; }
        IGenericRepository<Statistic> Statistics { get; }
        IGenericRepository<Match> Matches { get; }
        IGenericRepository<Zone> Zones { get; }




        Task<int> SaveChangesAsync(); // para guardar los cambios en la base de datos,
                                      // devuelve el número de filas afectadas
    }
}
