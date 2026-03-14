using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IStatisticRepository : IGenericRepository<Statistic>
    {
        //Segun la logica de negocio, lo unico que nos sirve es

        //buscar las estadisticas de una zona para armar las posiciones dentro de un grupo

        Task<IEnumerable<Statistic>> GetStatisticsByZoneIdAsync(Guid zoneId);

        //Obtener las estadisticas de una pareja para mostrar el perfil de la misma

        Task<IEnumerable<Statistic>> GetStatisticsByCoupleIdAsync(Guid coupleId);

        //Y traer las estadisticas de una pareja de una zona 

        Task<Statistic?> GetStatisticByCoupleIdAndZoneIdAsync(Guid coupleId, Guid zoneId);
    }
}
