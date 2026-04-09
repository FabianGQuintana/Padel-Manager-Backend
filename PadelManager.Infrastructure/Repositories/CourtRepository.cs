using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class CourtRepository : GenericRepository<Court>, ICourtRepository
    {
        public CourtRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<IEnumerable<Court>> GetCourtsByNameAsync(string name)
        {
            return await _context.Courts
                .Where(c => c.Name.ToLower() == name.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Court>> GetCourtsBySurfaceTypeAsync(string surfaceType)
        {
            return await _context.Courts
                .Where(c => c.SurfaceType.ToLower().Contains(surfaceType.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Court>> GetCourtsByVenueIdAsync(Guid venueId)
        {
            return await _context.Courts
             .Where(c => c.VenueId == venueId)
             .ToListAsync();
        }

        public async Task<IEnumerable<Court>> GetCourtsByAvailabilityAsync(CourtAvailabilityType availability)
        {
            
            return await _context.Courts
                .Where(c => c.CourtAvailability == availability)
                .ToListAsync();
        }
    }
}
