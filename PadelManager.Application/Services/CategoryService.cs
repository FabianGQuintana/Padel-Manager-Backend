using PadelManager.Application.DTOs.Category;
using PadelManager.Application.Interfaces.Common; 
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;
using System;

namespace PadelManager.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public CategoryService(
            ICategoryRepository categoryRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser) 
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
            // 1. Buscamos el torneo
            var tournament = await _unitOfWork.Tournaments.GetByIdAsync(dto.TournamentId);

            // 2. Validaciones de existencia y estado de BaseEntity
            if (tournament == null)
                throw new InvalidOperationException("El torneo especificado no existe.");

            // Validamos que el torneo esté "Activo" y no esté borrado (Soft Delete)
            // Usamos el atributo 'Status' y 'IsDeleted' de tu BaseEntity
            if (tournament.IsDeleted || tournament.Status != "Active")
            {
                throw new InvalidOperationException("No se pueden agregar categorías a un torneo que ha sido eliminado o desactivado.");
            }

            // 3. REGLA DE NEGOCIO: Solo en estado Draft
            if (tournament.StatusType != TournamentStatus.Draft)
            {
                throw new InvalidOperationException(
                    $"Las categorías solo pueden gestionarse mientras el torneo está en Borrador. " +
                    $"Estado actual: {tournament.StatusType}.");
            }
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
            // 1. Buscamos con sus hijos incluidos
            var category = await _categoryRepo.GetByIdWithChildrenAsync(id);
            if (category == null) return false;

            // 2. Si lo estamos por ELIMINAR (Soft Delete), validamos:
            if (!category.IsDeleted)
            {
                if (category.Stages.Any(s => !s.IsDeleted))
                    throw new InvalidOperationException("No se puede eliminar la categoría: tiene etapas activas.");

                if (category.Registrations.Any(r => !r.IsDeleted))
                    throw new InvalidOperationException("No se puede eliminar la categoría: ya tiene parejas inscritas.");
            }

            category.LastModifiedBy = _currentUser.UserName ?? "System";
            category.LastModifiedAt = DateTime.UtcNow;

            await _categoryRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id)
        {

            var category = await _categoryRepo.GetByIdWithChildrenAsync(id);
            return category?.ToResponseDto();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoryWithRegistrationsAsync(Guid id)
        {
            var categories = await _categoryRepo.GetCategoryWithRegistrationsAsync(id);
            return categories.ToResponseDto();
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
            if (maxTeams <6 || maxTeams >48)
            {
                 throw new InvalidOperationException("Las parejas tienen que tener un rango oficial de 6 hasta 48 por categoria.");
            }
            var categories = await _categoryRepo.GetCategoriesByMaxTeamsWithRegistrationsAsync(maxTeams);
            return categories.ToResponseDto();

        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllCategoriesWithDetailsAsync();
            return categories.ToResponseDto();
        }
    }
}