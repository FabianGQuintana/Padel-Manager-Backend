using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IStageRepository : IGenericRepository<Stage>
    {
        Task<IEnumerable<Stage>> GetStagesByCategoryIdAsync(Guid categoryId);
        Task<Stage?> GetStageByCategoryIdAndOrderAsync(Guid categoryId, int order);
        Task<Stage?> GetGroupStageByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Stage>> GetStagesByTypeAsync(StageType type);
        Task<Stage?> GetByIdWithChildrenAsync(Guid id);
    }
}