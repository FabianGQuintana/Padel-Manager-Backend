using PadelManager.Application.DTOs.Category;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;

namespace PadelManager.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepo, IUnitOfWork unitOfWork)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
        }

        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================

        public async Task<CategoryResponseDto> AddNewCategoryAsync(CreateCategoryDto dto)
        {
            // 1. Usamos el Mapper para convertir DTO -> Entidad
            var category = dto.ToEntity();

            // 2. Persistencia
            await _categoryRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            // 3. Devolvemos el ResponseDto
            return category.ToResponseDto();
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) return false;

            // 1. Mapeamos los cambios del DTO a la entidad existente
            existingCategory.MapToEntity(dto);

            // 2. Guardamos (La auditoría se encarga del LastModifiedAt)
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleCategoryAsync(Guid id)
        {
            // Usamos tu método genérico del repositorio
            var result = await _categoryRepo.SoftDeleteToggleAsync(id);
            if (result == null) return false;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id)
        {
            
            var category = await _categoryRepo.GetCategoryWithRegistrationsAsync(id);
            return category?.ToResponseDto();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByTournamentIdAsync(Guid tournamentId)
        {
            var categories = await _categoryRepo.GetCategoriesByTournamentWithRegistrationsAsync(tournamentId);
            return categories.ToResponseDto();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name)
        {
            var categories = await _categoryRepo.GetCategoriesByNameWithRegistrationsAsync(name);
            return categories.ToResponseDto();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByMaxTeamsAsync(int maxTeams)
        {
            var categories = await _categoryRepo.GetCategoriesByMaxTeamsWithRegistrationsAsync(maxTeams);
            return categories.ToResponseDto();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories.ToResponseDto();
        }
    }
}