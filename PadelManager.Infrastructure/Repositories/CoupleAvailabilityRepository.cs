using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PadelManager.Infrastructure.Repositories
{
    public class CoupleAvailabilityRepository : GenericRepository<CoupleAvailability>, ICoupleAvailabilityRepository
    {
       

        public CoupleAvailabilityRepository(PadelManagerDbContext context) : base(context)
        {
           
        }

        public async Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesByCoupleIdAsync(Guid coupleId)
        {
            return await _context.CoupleAvailabilities
                .Include(ca => ca.Couple)
                .Where(ca => ca.CoupleId == coupleId && ca.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesWithCouplesAsync()
        {
            return await _context.CoupleAvailabilities
                .Include(ca => ca.Couple)
                .Where(ca => ca.DeletedAt == null)
                .ToListAsync();
        }

        public async Task SoftDeleteAvailabilitiesByCoupleIdAsync(Guid coupleId)
        {
            var availabilities = await _context.CoupleAvailabilities
                .Where(ca => ca.CoupleId == coupleId && ca.DeletedAt == null)
                .ToListAsync();

            foreach (var availability in availabilities)
            {
                availability.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}