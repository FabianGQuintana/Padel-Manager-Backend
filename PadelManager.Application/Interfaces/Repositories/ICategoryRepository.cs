using System;
using PadelManager.Domain.Entities;


namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByNameAsync(string name);

        Task<IEnumerable<Category>> GetCategoryByDescriptionAsync(string description);

        Task<IEnumerable<Category>> GetCategoriesByTournamentIdAsync(Guid tournamentId);

        Task<IEnumerable<Category>> GetCategoriesByStageIdAsync(Guid stageId);

        Task<Category?> GetCategoryWithDetailsByIdAsync(Guid categoryId);
    }
}
