using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class StatisticRepository : GenericRepository<Statistic>, IStatisticRepository
    {
        

        public StatisticRepository(PadelManagerDbContext context) : base(context)
        {
          
        }

        public async Task<IEnumerable<Statistic>> GetStatisticsByZoneIdAsync(Guid zoneId)
        {
            return await _context.Set<Statistic>()
                .Include(s => s.Couple) // Mostrar los nombres de la pareja en la tabla de posiciones
                .Where(s => s.ZoneId == zoneId)
                //Esto es opcional pero sirve para ordenar directamente por puntos descendente para que la tabla ya venga armada
                .OrderByDescending(s => s.Points)          // 1º Criterio: El que tiene más puntos
                .ThenByDescending(s => s.MatchesWon)       // 2º Criterio (Desempate): El que ganó más partidos
                .ThenByDescending(s => s.SetsWon - s.SetsLost)
                .ThenByDescending(s => s.GamesWon - s.GamesLost)        // 3º Criterio (Desempate final): El que ganó más games
                .ToListAsync();
        }

        public async Task<IEnumerable<Statistic>> GetStatisticsByCoupleIdAsync(Guid coupleId)
        {
            return await _context.Set<Statistic>()
                .Include(s => s.Zone) // Opcional: para saber en qué zona hizo esos puntos
                .Where(s => s.CoupleId == coupleId)
                .ToListAsync();
        }

        public async Task<Statistic?> GetStatisticByCoupleIdAndZoneIdAsync(Guid coupleId, Guid zoneId)
        {
            return await _context.Set<Statistic>()
                .Where(s => s.CoupleId == coupleId && s.ZoneId == zoneId )
                .FirstOrDefaultAsync();
        }
    }
}