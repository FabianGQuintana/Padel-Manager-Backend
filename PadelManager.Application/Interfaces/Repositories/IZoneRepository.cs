using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        
        // Búsqueda por propiedades directas
        Task<Zone?> GetZoneByName(string name);
        Task<IEnumerable<Zone>> GetZonesByStageId(Guid stageId);
        
        //* Búsquedas relacionadas (Navigation Properties)
  
        // Obtener una zona incluyendo sus parejas y estadísticas
        Task<Zone?> GetZoneWithDetailsById(Guid zoneId);

        // Buscar todas las zonas en las que participa una pareja específica
        Task<IEnumerable<Zone>> GetZonesByCoupleId(Guid coupleId);
    }
}
