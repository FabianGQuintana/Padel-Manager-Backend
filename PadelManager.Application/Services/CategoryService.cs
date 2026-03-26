using PadelManager.Application.DTOs.Category;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common; // Agregado para ICurrentUser
using PadelManager.Application.Mappers;
using System;

namespace PadelManager.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser; //  Agregado

        public CategoryService(
            ICategoryRepository categoryRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser) //  Inyectado
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================

        public async Task<CategoryResponseDto> AddNewCategoryAsync(CreateCategoryDto dto)
        {
            var category = dto.ToEntity();

            // AUDITORÍA
            var user = _currentUser.UserName ?? "System";
            category.CreatedBy = user;
            category.LastModifiedBy = user;

            await _categoryRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.ToResponseDto();
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) return false;

            existingCategory.MapToEntity(dto);

            // AUDITORÍA
            existingCategory.LastModifiedBy = _currentUser.UserName ?? "System";
            existingCategory.LastModifiedAt = DateTime.UtcNow;

            // PERSISTENCIA (UoW ya estaba, ahora confirmamos el impacto)
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleCategoryAsync(Guid id)
        {
            // Buscamos la categoría primero para poder auditar quién hace el toggle
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            //  AUDITORÍA
            category.LastModifiedBy = _currentUser.UserName ?? "System";
            category.LastModifiedAt = DateTime.UtcNow;

            // Ejecutamos el toggle
            await _categoryRepo.SoftDeleteToggleAsync(id);

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