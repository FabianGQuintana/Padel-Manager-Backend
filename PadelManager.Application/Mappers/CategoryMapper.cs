using PadelManager.Application.DTOs.Category;
using PadelManager.Application.DTOs.Zone;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class CategoryMapper
    {

        public static CategoryResponseDto ToResponseDto(this Category category)
        {
            int count = category.Registrations?.Count ?? 0;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                MaxTeams = category.MaxTeams,
                TournamentId = category.TournamentId,
                RegistrationCount = count,
                IsActive = category.DeletedAt == null ? "Activo" : "Inactivo",
                // Usamos tus métodos de la entidad pura:
                IsFull = category.IsFull(count),
                CanStart = category.CanStart(count)

            };
        }

        public static IEnumerable<CategoryResponseDto> ToResponseDto(this IEnumerable<Category> categories)
        {
            return categories.Select(t => t.ToResponseDto());
        }

        public static void MapToEntity(this Category existingCategory, UpdateCategoryDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name) && existingCategory.Name != null)
            {
                existingCategory.Name = dto.Name.Trim();
            }
            
            if(dto.Description != null)
            {
                existingCategory.Description = dto.Description;
            }


            if (dto.MaxTeams.HasValue && dto.MaxTeams > 5 && dto.MaxTeams < 49)
            {
                existingCategory.MaxTeams = dto.MaxTeams.Value;
            }

        }


        public static Category ToEntity (this CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                MaxTeams = dto.MaxTeams,
                TournamentId = dto.TournamentId
            };
        }

    }
}
