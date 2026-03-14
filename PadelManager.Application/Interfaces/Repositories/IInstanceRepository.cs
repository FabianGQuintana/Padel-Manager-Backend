using System;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IInstanceRepository : IGenericRepository<Instance>
    {
        Task<IEnumerable<Instance>> GetInstancesByNameAsync(string name);

        Task<IEnumerable<Instance>> GetInstancesByDescriptionAsync(string description);

        Task<IEnumerable<Instance>> GetInstancesByStageIdAsync(Guid stageId);

        Task<Instance?> GetInstanceByMatchIdAsync(Guid matchId);
    }
}
