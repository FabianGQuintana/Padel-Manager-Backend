using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
    {
        private readonly PadelManagerDbContext _context;

        public ManagerRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Manager?> GetManagerWithUserAsync(Guid managerId)
        {
            return await _context.Managers
                .Include(m => m.User) // Eager Loading para traer los datos personales
                .FirstOrDefaultAsync(m => m.Id == managerId && m.DeletedAt == null);
        }

        public async Task<Manager?> GetManagerByUserIdAsync(Guid userId)
        {
            // Como usamos Shared Primary Key, buscamos por ID directamente
            return await _context.Managers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.DeletedAt == null);
        }
    }
}