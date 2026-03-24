using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Repositories
{
    public class CoupleAvailabilityRepository : GenericRepository<CoupleAvailability>, ICoupleAvailabilityRepository
    {
        private readonly PadelManagerDbContext _context;

        public CoupleAvailabilityRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesByCoupleIdAsync(Guid coupleId)
        {
            return await _context.CoupleAvailabilities
                .Include(ca => ca.Couple)
                .Where(ca => ca.CoupleId == coupleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesWithCouplesAsync()
        {
            return await _context.CoupleAvailabilities
                .Include(ca => ca.Couple)
                .ToListAsync();
        }

        public async Task DeleteAvailabilitiesByCoupleIdAsync(Guid coupleId)
        {
            var availabilities = await _context.CoupleAvailabilities
                .Where(ca => ca.CoupleId == coupleId)
                .ToListAsync();

            foreach (var availability in availabilities)
            {
                availability.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }



    }
}
