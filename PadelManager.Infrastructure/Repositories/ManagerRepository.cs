using PadelManager.Application.Interfaces.Repositories;
using System;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace PadelManager.Infrastructure.Repositories
{
    public class ManagerRepository : GenericRepository<Manager> , IManagerRepository
    {
        private readonly PadelManagerDbContext _context;

        public ManagerRepository(PadelManagerDbContext context) : base(context)
        {
           //Aca ya no hace falta inicializar el _context porque ya lo hace el constructor de la clase base (GenericRepository) pero lo dejo por claridad. 
            _context = context;
        }

        //Implementación de los métodos específicos para el repositorio de Managers
        public async Task<IEnumerable<Manager>> GetManagersByNameAsync(string nameManager)
        {
            return await _context.Managers
                .Where(m => m.Name == nameManager)
                .ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetManagersByLastNameAsync(string lastName)
        {
            return await _context.Managers
                .Where(m =>m.LastName == lastName)
                .ToListAsync();
        }

        public async Task<Manager?> GetManagerByDNIAsync (string dniManager)
        {
            return await _context.Managers
            .FirstOrDefaultAsync(m => m.Dni == dniManager);
            //Ambas formas son correctas, la primera es mas legible pero la segunda es mas concisa.
        }

        public async Task<Manager?> GetManagerByEmailAsync(string emailManager)
        {
            return await _context.Managers
                .Where(m => m.Email == emailManager)
                .FirstOrDefaultAsync();
        }

        public async Task<Manager?> GetManagerByPhoneAsync(string phoneManager)
        {
            return await _context.Managers
                .Where(m => m.PhoneNumber == phoneManager)
                .FirstOrDefaultAsync();
        }

        public async Task<Manager?> GetManagerWithTournamentsAsync(Guid id)
        {
            return await _context.Managers
                .Include(m => m.Tournaments)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);
        }

    }
}
