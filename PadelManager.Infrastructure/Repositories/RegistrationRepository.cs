using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelManager.Domain.Entities;
using PadelManager.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
    {
        private new readonly PadelManagerDbContext _context;

        public RegistrationRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Couple>> GetCouplesByCategoryAsync(Guid categoryId)
        {
            return await _context.Registrations
                .Include(r => r.Couple) 
                .Where(r => r.CategoryId == categoryId )
                .Select(r => r.Couple) 
                .Distinct()        
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByDateAsync(DateTime date)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationDate == DateOnly.FromDateTime(date) )
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByTimeAsync(TimeOnly time)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationTime == time )
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCoupleIdAsync(Guid coupleId)
        {
            return await _context.Registrations
                .Where(r => r.CoupleId == coupleId )
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Registrations
                .Where(r => r.CategoryId == categoryId )
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.Registrations
                .AsNoTracking() 
                .Include(r => r.Category)
                .Include(r => r.Couple)
                    .ThenInclude(c => c.Player1)
                .Include(r => r.Couple)
                    .ThenInclude(c => c.Player2)
                .Where(r => r.TournamentId == tournamentId && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByDetailsAsync(
            DateTime date,
            TimeOnly time,
            Guid coupleId,
            Guid categoryId)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationDate == DateOnly.FromDateTime(date) &&
                            r.RegistrationTime == time &&
                            r.CoupleId == coupleId &&
                            r.CategoryId == categoryId 
                           )
                .ToListAsync();
        }

        public async Task<int> CountRegistrationsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Registrations
                .CountAsync(r => r.CategoryId == categoryId );
        }

        public async Task<int> CountByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.Registrations
                .CountAsync(r => r.TournamentId == tournamentId );
        }

        public async Task<bool> ExistsByCoupleAndTournamentAsync(Guid coupleId, Guid tournamentId)
        {
            return await _context.Registrations
                .AnyAsync(r => r.CoupleId == coupleId &&
                               r.TournamentId == tournamentId
                              );
        }

        public async Task<Registration?> GetRegistrationByIdWithDetailsAsync(Guid id)
        {
            return await _context.Registrations
                .Include(r => r.Category)
                .Include(r => r.Tournament)
                .Include(r => r.Couple)
                    .ThenInclude(c => c.Player1) 
                .Include(r => r.Couple)
                    .ThenInclude(c => c.Player2)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


      
        public async Task<bool> IsAnyPlayerAlreadyRegisteredInTournamentAsync(Guid tournamentId, Guid player1Id, Guid player2Id)
        {
            return await _context.Registrations
                .AsNoTracking()
                .Include(r => r.Couple)
                .AnyAsync(r => r.TournamentId == tournamentId &&
                               r.DeletedAt == null && 
                               (r.Couple.Player1Id == player1Id ||
                                r.Couple.Player1Id == player2Id ||
                                r.Couple.Player2Id == player1Id ||
                                r.Couple.Player2Id == player2Id));
        }
    }
}