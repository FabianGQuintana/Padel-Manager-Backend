using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Domain.Enum;

namespace PadelManager.Infrastructure.Repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        private readonly PadelManagerDbContext _context;

        public MatchRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        // ========================================================================
        // Implementación de métodos específicos para el repositorio de Partidos
        // ========================================================================

        #region Búsquedas por propiedades directas

        public async Task<IEnumerable<Match>> GetMatchesByDate(DateTime date)
        {
            // Usamos .Date para comparar solo el día y no la hora exacta
            return await _context.Matches
                .Where(m => m.DateTime.Date == date.Date && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByStatus(MatchStatus status)
        {
            return await _context.Matches
                .Where(m => m.Status == status && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByLocation(string locationName)
        {
            return await _context.Matches
                .Where(m => m.LocationName == locationName && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByCourt(string courtName)
        {
            return await _context.Matches
                .Where(m => m.CourtName == courtName && m.DeletedAt == null)
                .ToListAsync();
        }

        #endregion

        #region Búsquedas con relaciones y FKs

        public async Task<IEnumerable<Match>> GetMatchesByInstanceId(Guid instanceId)
        {
            return await _context.Matches
                .Where(m => m.InstanceId == instanceId && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByCoupleId(Guid coupleId)
        {
            // OJO ACÁ: Se usa el operador OR (||) porque la pareja buscada puede ser la Couple 1 o la Couple 2
            return await _context.Matches
                .Where(m => (m.CoupleId == coupleId || m.CoupleId2 == coupleId) && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchWithDetailsById(Guid matchId)
        {
            // Traemos el partido e incluimos las tablas relacionadas para tener los datos completos
            return await _context.Matches
                .Include(m => m.Instance)
                .Include(m => m.Couple)
                .Include(m => m.Couple2)
                .Where(m => m.Id == matchId && m.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}
