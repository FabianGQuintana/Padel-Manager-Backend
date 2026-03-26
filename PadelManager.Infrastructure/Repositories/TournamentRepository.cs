using System;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Domain.Enum;

namespace PadelManager.Infrastructure.Repositories
{
    public class TournamentRepository : GenericRepository<Tournament> , ITournamentRepository
    {
        

        public TournamentRepository(PadelManagerDbContext context) : base(context)
        {

        }

        //Implementación de los métodos específicos para el repositorio de torneos

        public async Task<IEnumerable<Tournament>> GetTournamentsByNameAsync(string name)
        {
            return await _context.Tournaments
                .Include(t => t.Categories) // Incluir las categorías relacionadas
                .Where(t => t.Name.ToLower() == name.ToLower() && t.DeletedAt == null) //De esta forma me puedo asegurar de filtrar los borrados lógicamente
                .ToListAsync();
        }


        public async Task<IEnumerable<Tournament>> GetTournamentsByStartDateAsync(DateTime startDate)
        {
            return await _context.Tournaments // Me aseguro de mirar la fecha unicamente y no la hora , por eso el .Date chicas
                .Where (t => t.StartDate.Date == startDate.Date && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByStatusAsync(TournamentStatus status)
        {
            return await _context.Tournaments
                .Where(t => t.StatusType == status && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByMaxTeamsAsync(int maxTeams)
        {
            return await _context.Tournaments
                .Include(t => t.Categories) // Importante cargar las categorías
                .Where(t => t.Categories.Any(c => c.MaxTeams == maxTeams) && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByTypeAsync(string tournamentType)
        {
            return await _context.Tournaments
                .Where(t => t.TournamentType == tournamentType && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerIdAsync(Guid managerId)
        {
            return await _context.Tournaments
                .Where(t => t.ManagerId == managerId && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Tournaments
                .Where(t => t.Categories.Any(c => c.Id == categoryId) && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerEmailAsync(string email)
        {
            return await _context.Tournaments
                .Include(t => t.Managers) // Asumiendo la relación en tu entidad
                .Where(t => t.Managers.Any(m => m.Email == email && t.DeletedAt == null))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerDniAsync(string dni)
        {
            return await _context.Tournaments
            .Include(t => t.Managers)
            .Where(t => t.DeletedAt == null && t.Managers.Any(m => m.Dni == dni)) 
            .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerNameAsync(string name)
        {
            return await _context.Tournaments
                .Include(t => t.Managers)
                .Where(t => t.DeletedAt == null &&
                       t.Managers.Any(m => (m.Name + " " + m.LastName).ToLower().Contains(name.ToLower())))
                .ToListAsync();
        }

        public async Task<Tournament?> GetTournamentWithCategoriesAsync(Guid id)
        {
            return await _context.Tournaments
                .Include(t => t.Categories)
                .Where(t => t.Id == id && t.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
    }
}
