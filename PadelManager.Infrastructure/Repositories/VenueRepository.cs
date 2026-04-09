using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using System.Runtime.Loader;
namespace PadelManager.Infrastructure.Repositories
{
    public class VenueRepository : GenericRepository<Venue> , IVenueRepository
    {
        public VenueRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<Venue?> GetVenueWithCourtsAsync(Guid id)
        {
            return await _context.Venues
                .Include(v => v.Courts)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venue>> GetVenuesByCityAsync(string city)
        {
            return await _context.Venues
                .Where(v => v.City.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<Venue?> GetVenueByAddressAsync(string address)
        {
            return await _context.Venues
                .Where(v => v.Address == address)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Venue>> GetVenueByNameAsync(string name)
        {
            return await _context.Venues
                .Where(v => v.Name == name)
                .ToListAsync();

        }

        public async Task<Venue?> GetVenueByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Venues
                .Where(v => v.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();

        }
    }
}
