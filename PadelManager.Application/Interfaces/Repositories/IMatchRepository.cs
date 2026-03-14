using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        
        // Búsquedas por propiedades directas
        
        Task<IEnumerable<Match>> GetMatchesByDate(DateTime date);

        Task<IEnumerable<Match>> GetMatchesByStatus(MatchStatus status);

        Task<IEnumerable<Match>> GetMatchesByLocation(string locationName);

        Task<IEnumerable<Match>> GetMatchesByCourt(string courtName);

        // --------------------------------------------------------
        // Búsquedas con relaciones (Navigation Properties y FKs)
        // --------------------------------------------------------

        // Busca los partidos que pertenecen a una instancia específica (ej: Semifinales)
        Task<IEnumerable<Match>> GetMatchesByInstanceId(Guid instanceId);

        // Busca todos los partidos donde juegue una pareja en particular (ya sea la pareja 1 o la 2)
        Task<IEnumerable<Match>> GetMatchesByCoupleId(Guid coupleId);

        // Obtiene un partido con toda la información de las parejas y la instancia cargadas
        Task<Match?> GetMatchWithDetailsById(Guid matchId);
    }
}