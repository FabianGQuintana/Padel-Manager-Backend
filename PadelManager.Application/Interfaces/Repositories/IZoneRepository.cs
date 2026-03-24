using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
       
        Task<IEnumerable<Zone>> GetZonesByNameAsync(string name);
        Task<IEnumerable<Zone>> GetZonesByStageIdAsync(Guid stageId);
        
        //* Búsquedas relacionadas (Navigation Properties)
  
        // Obtener una zona incluyendo sus parejas y estadísticas
        Task<Zone?> GetZoneWithDetailsByIdAsync(Guid zoneId);

        // Buscar todas las zonas en las que participa una pareja específica
        Task<IEnumerable<Zone>> GetZonesByCoupleIdAsync(Guid coupleId);
    }
}
