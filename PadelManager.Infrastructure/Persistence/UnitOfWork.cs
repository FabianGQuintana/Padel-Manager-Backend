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
        private IGenericRepository<Player>? _players;
        private IGenericRepository<Match>? _matches;
        private IGenericRepository<Manager>? _managers;
        private IGenericRepository<Tournament>? _tournaments;
        private IGenericRepository<Zone>? _zones;
        private IGenericRepository<Statistic>? _statistics;
        private IGenericRepository<Stage>? _stages;
        private IGenericRepository<Couple>? _couples;
        private IGenericRepository<Registration>? _registrations;
        private IGenericRepository<Category>? _categories;
        private IGenericRepository<CoupleAvailability>? _coupleAvailabilities;

        public UnitOfWork(PadelManagerDbContext context)
        {
            _context = context;
        }

        // Propiedad para acceder al repositorio de Players
        public IGenericRepository<Player> Players => _players ??= new GenericRepository<Player>(_context);

        // Propiedad para acceder al repositorio de Matches
        public IGenericRepository<Match> Matches => _matches ??= new GenericRepository<Match>(_context);

        public IGenericRepository<Manager> Managers => _managers ??= new GenericRepository<Manager>(_context);

        public IGenericRepository<Tournament> Tournaments => _tournaments ??= new GenericRepository<Tournament>(_context);

        public IGenericRepository<Couple> Couples => _couples ??= new GenericRepository<Couple>(_context);

        public IGenericRepository<Statistic> Statistics => _statistics ??= new GenericRepository<Statistic>(_context);

        public IGenericRepository<Stage> Stages => _stages ??= new GenericRepository<Stage>(_context);

        public IGenericRepository<Zone> Zones => _zones ??= new GenericRepository<Zone>(_context);

        public IGenericRepository<Registration> Registrations => _registrations ??= new GenericRepository<Registration>(_context);

        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);

        // Guarda todos los cambios pendientes en la DB
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this); // Buena práctica para liberar memoria
        }

    }
}
