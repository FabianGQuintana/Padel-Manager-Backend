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
    public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
    {
        

        public ZoneRepository(PadelManagerDbContext context) : base(context)
        {
            
        }

        // ========================================================================
        // Implementación de métodos específicos para el repositorio de Zonas
        // ========================================================================

        #region Búsquedas por propiedades directas

        public async Task<IEnumerable<Zone>> GetZonesByNameAsync(string name)
        {
            return await _context.Zones
                .Where(z => z.Name == name ) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Zone>> GetZonesByStageIdAsync(Guid stageId)
        {
            return await _context.Zones
                .Where(z => z.StageId == stageId )
                .ToListAsync();
        }

        #endregion

        #region Búsquedas con relaciones (Navigation Properties)

        public async Task<Zone?> GetZoneWithDetailsByIdAsync(Guid zoneId)
        {
            return await _context.Zones
                .Include(z => z.Stage)
                .Include(z => z.Matches)
                .Include(z => z.Couples)     
                .Include(z => z.Statistics)  
                .Where(z => z.Id == zoneId )
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Zone>> GetZonesByCoupleIdAsync(Guid coupleId)
        {
            // Usamos .Any() para buscar dentro de la colección de parejas de la zona
            return await _context.Zones
                .Where(z => z.Couples.Any(c => c.Id == coupleId) )
                .ToListAsync();
        }

        public async Task<int> CountByStageIdAsync(Guid stageId)
        {
            // Realiza un SELECT COUNT(*) directamente en la base de datos
            return await _context.Zones
                .CountAsync(z => z.StageId == stageId );
        }


        #endregion
    }
}
