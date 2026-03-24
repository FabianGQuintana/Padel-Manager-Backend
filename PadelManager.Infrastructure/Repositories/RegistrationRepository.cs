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
                .Where(r => r.CategoryId == categoryId && r.DeletedAt == null)
                .Select(r => r.Couple) 
                .Distinct()        
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByDateAsync(DateTime date)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationDate == DateOnly.FromDateTime(date) && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByTimeAsync(TimeOnly time)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationTime == time && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCoupleIdAsync(Guid coupleId)
        {
            return await _context.Registrations
                .Where(r => r.CoupleId == coupleId && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Registrations
                .Where(r => r.CategoryId == categoryId && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.Registrations
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
                            r.CategoryId == categoryId &&
                            r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<int> CountRegistrationsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Registrations
                .CountAsync(r => r.CategoryId == categoryId && r.DeletedAt == null);
        }

        public async Task<int> CountByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.Registrations
                .CountAsync(r => r.TournamentId == tournamentId && r.DeletedAt == null);
        }

        public async Task<bool> ExistsByCoupleAndTournamentAsync(Guid coupleId, Guid tournamentId)
        {
            return await _context.Registrations
                .AnyAsync(r => r.CoupleId == coupleId &&
                               r.TournamentId == tournamentId &&
                               r.DeletedAt == null);
        }
    }
}