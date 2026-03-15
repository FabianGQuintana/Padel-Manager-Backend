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
        private readonly PadelManagerDbContext _context;

        public ZoneRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context; // Mantenemos la asignación local por claridad
        }

        // ========================================================================
        // Implementación de métodos específicos para el repositorio de Zonas
        // ========================================================================

        #region Búsquedas por propiedades directas

        public async Task<Zone?> GetZoneByNameAsync(string name)
        {
            return await _context.Zones
                .Where(z => z.Name == name && z.DeletedAt == null) // Filtro de borrado lógico
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Zone>> GetZonesByStageIdAsync(Guid stageId)
        {
            return await _context.Zones
                .Where(z => z.StageId == stageId && z.DeletedAt == null)
                .ToListAsync();
        }

        #endregion

        #region Búsquedas con relaciones (Navigation Properties)

        public async Task<Zone?> GetZoneWithDetailsByIdAsync(Guid zoneId)
        {
            return await _context.Zones
                .Include(z => z.Stage)       // Incluimos la etapa a la que pertenece
                .Include(z => z.Couples)     // Incluimos la lista de parejas
                .Include(z => z.Statistics)  // Incluimos las estadísticas
                .Where(z => z.Id == zoneId && z.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Zone>> GetZonesByCoupleIdAsync(Guid coupleId)
        {
            // Usamos .Any() para buscar dentro de la colección de parejas de la zona
            return await _context.Zones
                .Where(z => z.Couples.Any(c => c.Id == coupleId) && z.DeletedAt == null)
                .ToListAsync();
        }

        #endregion
    }
}
