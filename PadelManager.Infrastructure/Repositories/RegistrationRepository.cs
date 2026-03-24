using System;
using PadelManager.Domain.Entities;
using PadelManager.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
    {

        private readonly PadelManagerDbContext _context;

        public RegistrationRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        //Implementación de métodos específicos para la entidad Registration
        public async Task<IEnumerable<Registration>> GetRegistrationsByDateAsync(DateTime date)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationDate == DateOnly.FromDateTime(date) && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<int> CountByTournamentIdAsync(Guid tournamentId)
        {
            // Buscamos todas las registraciones que pertenezcan a las categorías de ese torneo
            return await _context.Registrations
                .CountAsync(r => r.Category.TournamentId == tournamentId && r.DeletedAt == null);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByTimeAsync(TimeOnly time)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationTime == time && r.DeletedAt == null)
                .ToListAsync();

        }

        public async Task<Registration?> GetRegistrationByCoupleIdAsync(Guid coupleId)
        {
            return await _context.Registrations
                .Where(r => r.CoupleId == coupleId && r.DeletedAt == null)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Registration>> GetRegistrationsByCategoryId(Guid categoryId)
        {
            return await _context.Registrations
                .Where(r => r.CategoryId == categoryId && r.DeletedAt == null)
                .ToListAsync();

        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByDetailsAsync(DateTime date, TimeOnly time, Guid coupleId, Guid categoryId)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationDate == DateOnly.FromDateTime(date) &&
                            r.RegistrationTime == time &&
                            r.CoupleId == coupleId &&
                            r.CategoryId == categoryId && r.DeletedAt == null)
                            .ToListAsync();
        }

        public async Task<int> CountRegistrationsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Registrations
                .CountAsync(r => r.CategoryId == categoryId && r.DeletedAt == null);
        }

        public async Task<List<Couple>> GetCouplesByCategoryAsync(Guid categoryId)
        {
            return await _context.Registrations
                .Include(r => r.Couple) 
                .Where(r => r.CategoryId == categoryId && r.DeletedAt == null)
                .Select(r => r.Couple) 
                .ToListAsync();
        }

    }
}
