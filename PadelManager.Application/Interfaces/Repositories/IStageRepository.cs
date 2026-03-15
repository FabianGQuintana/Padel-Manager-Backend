using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IStageRepository : IGenericRepository<Stage>
    {
        Task <IEnumerable<Stage>> GetStagesByCategoryIdAsync(Guid categoryId);
        Task<Stage?> GetStageByCategoryIdAndOrderAsync(Guid categoryId, int order);
    }
}
