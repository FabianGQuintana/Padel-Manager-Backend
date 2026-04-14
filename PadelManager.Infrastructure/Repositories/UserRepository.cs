using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        

        public UserRepository(PadelManagerDbContext context) : base(context)
        {
            
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role) // Importante para saber qué permisos tiene al loguearse
                .FirstOrDefaultAsync(u => u.Email == email );
        }

        public async Task<User?> GetUserByDniAsync(string dni)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Dni == dni );
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber ==  phoneNumber );
        }

        public async Task<User?> GetUserWithManagerProfileAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Manager) // Traemos la info de LicenciaAPA, Experiencia, etc.
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId );
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
        {
            return await _context.Users
                .Where(u => u.Name == name )
                .ToListAsync();

        }

        public async Task<IEnumerable<User>> GetUsersByLastNameAsync(string lastName)
        {
            return await _context.Users
                .Where(u => u.LastName == lastName )
                .ToListAsync();

        }

        public async Task<User?> GetUserByIdWithRoleAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id );
        }

        public async Task<IEnumerable<User>> GetUsersByRoleNameAsync(string roleName)
        {           
            return await _context.Users
                .Include(u => u.Role) 
                .Where(u => u.Role.NameRol == roleName)
                .ToListAsync();
        }


        public async Task AddRefreshTokenAsync(RefreshToken token)
            => await _context.Set<RefreshToken>().AddAsync(token);

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
            => await _context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token);

        public async Task UpdateRefreshTokenAsync(RefreshToken token)
            => _context.Set<RefreshToken>().Update(token);

    }
}
