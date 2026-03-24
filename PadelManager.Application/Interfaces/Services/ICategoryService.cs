using PadelManager.Application.DTOs.Category;
using PadelManager.Application.DTOs.Tournament;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> AddNewCategoryAsync(CreateCategoryDto categoryDto);
        Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto);
        Task<bool> SoftDeleteToggleCategoryAsync(Guid id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();

        Task<CategoryResponseDto?> GetCategoryByIdAsync (Guid id);

        Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name);

        Task<IEnumerable<CategoryResponseDto>> GetCategoriesByMaxTeamsAsync(int maxTeams);

        Task<IEnumerable<CategoryResponseDto>> GetCategoriesByTournamentIdAsync(Guid tournamentId);



    }
}
