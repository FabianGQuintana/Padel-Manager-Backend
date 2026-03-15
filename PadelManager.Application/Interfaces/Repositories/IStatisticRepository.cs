using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IStatisticRepository : IGenericRepository<Statistic>
    {

        Task<IEnumerable<Statistic>> GetStatisticsByZoneIdAsync(Guid zoneId);

        Task<IEnumerable<Statistic>> GetStatisticsByCoupleIdAsync(Guid coupleId);

        Task<Statistic?> GetStatisticByCoupleIdAndZoneIdAsync(Guid coupleId, Guid zoneId);
    }
}
