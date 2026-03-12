using System;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PadelManager.Infrastructure.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly PadelManagerDbContext _context;

        public PlayerRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementación de métodos específicos para Player
        public async Task<IEnumerable<Player>> GetPlayerByNameAsync(string name)
        {
            return await _context.Players
                .Where(p => p.Name == name && p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayerByLastNameAsync(string lastName)
        {
            return await _context.Players
                .Where(p => p.LastName == lastName && p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Player?> GetPlayerByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Players
                .Where(p => p.PhoneNumer == phoneNumber && p.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<Player?> GetPlayerByDniAsync(int dni)
        {
            return await _context.Players
                .Where(p => p.Dni == dni && p.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayerByAgeAsync(Byte age)
        {
            return await _context.Players
                .Where(p => p.Age == age && p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayerByAvailabilityAsync(string availability)
        {
            return await _context.Players
                .Where(p => p.Availability == availability && p.DeletedAt == null)
                .ToListAsync();

        }
    }
}
