using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Infrastructure.Repositories;
using System;

namespace PadelManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly PadelManagerDbContext _context;

        // Campos privados para los repositorios (Lazy Initialization)
        private IGenericRepository<Player> _players;
        private IGenericRepository<Match> _matches;
        private IGenericRepository<Manager> _managers;
        public UnitOfWork(PadelManagerDbContext context)
        {
            _context = context;
        }

        // Propiedad para acceder al repositorio de Players
        public IGenericRepository<Player> Players => _players ??= new GenericRepository<Player>(_context);

        // Propiedad para acceder al repositorio de Matches
        public IGenericRepository<Match> Matches => _matches ??= new GenericRepository<Match>(_context);

        public IGenericRepository<Manager> Managers => _managers ??= new GenericRepository<Manager>(_context);

        // Guarda todos los cambios pendientes en la DB
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
