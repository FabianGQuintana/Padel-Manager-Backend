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
                .Where(t => t.Name.ToLower() == name.ToLower()) //De esta forma me puedo asegurar de filtrar los borrados lógicamente
                .ToListAsync();
        }


        public async Task<IEnumerable<Tournament>> GetTournamentsByStartDateAsync(DateTime startDate)
        {
            return await _context.Tournaments // Me aseguro de mirar la fecha unicamente y no la hora , por eso el .Date chicas
                .Where (t => t.StartDate.Date == startDate.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByStatusAsync(TournamentStatus status)
        {
            return await _context.Tournaments
                .Where(t => t.StatusType == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByMaxTeamsAsync(int maxTeams)
        {
            return await _context.Tournaments
                .Include(t => t.Categories) // Importante cargar las categorías
                .Where(t => t.Categories.Any(c => c.MaxTeams == maxTeams))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByTypeAsync(string tournamentType)
        {
            return await _context.Tournaments
                .Where(t => t.TournamentType == tournamentType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerIdAsync(Guid managerId)
        {
            return await _context.Tournaments
                .Where(t => t.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Tournaments
                .Where(t => t.Categories.Any(c => c.Id == categoryId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerEmailAsync(string email)
        {
            return await _context.Tournaments
                .Include(t => t.Managers)
                    .ThenInclude(m => m.User) //  Cargamos el User dentro de los Managers
                .Where(t => 
                       t.Managers.Any(m => m.User.Email == email)) //  Acceso vía m.User
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerDniAsync(string dni)
        {
            return await _context.Tournaments
                .Include(t => t.Managers)
                    .ThenInclude(m => m.User)
                .Where(t => 
                       t.Managers.Any(m => m.User.Dni == dni)) // Acceso vía m.User
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerNameAsync(string name)
        {
            return await _context.Tournaments
                .Include(t => t.Managers)
                    .ThenInclude(m => m.User)
                .Where(t =>
                       t.Managers.Any(m => (m.User.Name + " " + m.User.LastName)
                                            .ToLower().Contains(name.ToLower()))) //Acceso vía m.User
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetAllWithManagersAsync()
        {
            return await _context.Tournaments
                .Include(t => t.Managers)         //  Entramos a la colección de Managers
                .ThenInclude(m => m.User)     //  Entramos al User para sacar el nombre
                .ToListAsync();
        }


        public async Task<Tournament?> GetTournamentByIdWithManagersAsync(Guid id)
        {
            return await _context.Tournaments
                .Include(t => t.Managers)         // Cargamos Managers
                    .ThenInclude(m => m.User)     // Cargamos el User dentro del Manager
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tournament?> GetTournamentWithCategoriesAsync(Guid id)
        {
            return await _context.Tournaments
                .Include(t => t.Categories)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Tournament?> GetTournamentAccountingAsync(Guid tournamentId)
        {
            return await _context.Tournaments
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Payments)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Couple)
                        .ThenInclude(c => c.Player1) 
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Couple)
                        .ThenInclude(c => c.Player2) 
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Couple)
                        .ThenInclude(c => c.Availabilities)
                .Include(t => t.Categories)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }

    }
}
