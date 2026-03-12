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
        private readonly PadelManagerDbContext _context;

        public TournamentRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context; //No hace falta, ya recibe el contexto en el constructor de la clase base, pero lo dejo por claridad.
        }

        //Implementación de los métodos específicos para el repositorio de torneos

        public async Task<Tournament?> GetTournamentByName(string name)
        {
            return await _context.Tournaments
                .Include(t => t.Categories) // Incluir las categorías relacionadas
                .Where(t => t.Name == name && t.DeletedAt == null) //De esta forma me puedo asegurar de filtrar los borrados lógicamente
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Tournament>> GetTournamentByStartDate(DateTime startDate)
        {
            return await _context.Tournaments // Me aseguro de mirar la fecha unicamente y no la hora , por eso el .Date chicas
                .Where (t => t.StartDate.Date == startDate.Date && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentByStatus(TournamentStatus status)
        {
            return await _context.Tournaments
                .Where(t => t.Status == status && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentByType(string tournamentType)
        {
            return await _context.Tournaments
                .Where(t => t.TournamentType == tournamentType && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentByMaxTeamsPerCategory(int maxTeams)
        {
            return await _context.Tournaments
                .Where(t => t.MaxTeamsPerCategory == maxTeams && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByManagerId(Guid managerId)
        {
            return await _context.Tournaments
                .Where(t => t.ManagerId == managerId && t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByCategoryId(Guid categoryId)
        {
            return await _context.Tournaments
                .Where(t => t.Categories.Any(c => c.Id == categoryId) && t.DeletedAt == null)
                .ToListAsync();
        }

    }
}
