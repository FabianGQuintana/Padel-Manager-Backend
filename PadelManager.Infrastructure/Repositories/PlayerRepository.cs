using System;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PadelManager.Infrastructure.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
   

        public PlayerRepository(PadelManagerDbContext context) : base(context)
        {
            
        }

        // Implementación de métodos específicos para Player
        public async Task<IEnumerable<Player>> GetPlayerByNameAsync(string name)
        {
            return await _context.Players
                .Where(p => p.Name == name )
                .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayerByLastNameAsync(string lastName)
        {
            return await _context.Players
                .Where(p => p.LastName == lastName )
                .ToListAsync();
        }

        public async Task<Player?> GetPlayerByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Players
                .Where(p => p.PhoneNumber == phoneNumber )
                .FirstOrDefaultAsync();
        }

        public async Task<Player?> GetPlayerByDniAsync(string dni)
        {
            return await _context.Players
                .Where(p => p.Dni == dni )
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayersByAvailabilityAsync(string availability)
        {
            return await _context.Players
                .AsNoTracking()
                .Where(p => p.Availability != null &&
                            p.Availability.Contains(availability))
                .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayerByAgeAsync(Byte age)
        {
            return await _context.Players
                .Where(p => p.Age == age )
                .ToListAsync();
        }


        
    }
}
