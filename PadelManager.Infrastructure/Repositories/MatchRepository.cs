using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PadelManager.Infrastructure.Repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
       

        public MatchRepository(PadelManagerDbContext context) : base(context)
        {
           
        }

        // ========================================================================
        // Implementación de métodos específicos para el repositorio de Partidos
        // ========================================================================


        public async Task<IEnumerable<Match>> GetMatchesByWinnerAsync(Guid coupleId)
        {
            return await _context.Matches
                .Where(m => m.WinnerCoupleId == coupleId && m.DeletedAt == null)
                .ToListAsync();
        }
        public async Task<IEnumerable<Match>> GetMatchesByPlayerIdAsync(Guid playerId)
        {
            // Buscamos partidos donde el jugador esté en cualquiera de las dos parejas
            return await _context.Matches
                .Where(m => (m.Couple.Player1Id == playerId || m.Couple.Player2Id == playerId ||
                             m.Couple2.Player1Id == playerId || m.Couple2.Player2Id == playerId) 
                            && m.DeletedAt == null)
                .ToListAsync();
        }


        public async Task<IEnumerable<Match>> GetMatchesByStageIdAsync(Guid stageId)
        {
            return await _context.Matches
                .Where(m => m.StageId == stageId && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByZoneIdAsync(Guid zoneId)
        {
            return await _context.Matches
                .Where(m => m.ZoneId == zoneId && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByCoupleIdAsync(Guid coupleId)
        {
           
            return await _context.Matches
                .Where(m => (m.CoupleId == coupleId || m.CoupleId2 == coupleId) && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchWithDetailsByIdAsync(Guid matchId)
        {
            return await _context.Matches
                .Include(m => m.Stage)
                .Include(m => m.Zone) 
                .Include(m => m.Couple)
                .Include(m => m.Couple2)
                .FirstOrDefaultAsync(m => m.Id == matchId && m.DeletedAt == null);
        }

        public async Task<IEnumerable<Match>> GetMatchesByDateAsync(DateTime date)
        {
            // Usamos .Date para comparar solo el día, ignorando la hora exacta del partido
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.DateTime.Date == date.Date && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByLocationAsync(string location)
        {
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.LocationName == location && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByCourtAsync(string courtName)
        {
            // Ajustamos el nombre para que coincida exactamente con tu interfaz
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.CourtName == courtName && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByStatusAsync(MatchStatus status)
        {
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.StatusType == status && m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<int> CountByStageIdAsync(Guid stageId)
        {
            // Solo contamos los partidos que no han sido eliminados (Soft Delete)
            return await _context.Matches
                .CountAsync(m => m.StageId == stageId && m.DeletedAt == null);
        }

    }
}
