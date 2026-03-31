using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly PadelManagerDbContext _context;

        public RoleRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(TypeUser roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.NameRol == roleName && r.DeletedAt == null);
        }
    }
}