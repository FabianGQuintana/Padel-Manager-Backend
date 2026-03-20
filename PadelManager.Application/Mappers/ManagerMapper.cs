using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.DTOs.Tournament;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class ManagerMapper
    {
        public static void MapToEntity(this Manager existingEntity, UpdateManagerDto dto)
        {
             if(dto.Name != null) existingEntity.Name = dto.Name;
             if(dto.LastName != null) existingEntity.LastName = dto.LastName;
             if(dto.Dni != null) existingEntity.Dni = dto.Dni;
             if(dto.PhoneNumber != null) existingEntity.PhoneNumber = dto.PhoneNumber;
             if(dto.Email != null) existingEntity.Email = dto.Email;

        }

        public static Manager ToEntity(this CreateManagerDto dto)
        {
            return new Manager
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Dni = dto.Dni,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email

            };
        }

        public static ManagerResponseDto ToResponseDto(this Manager manager)
        {
            return new ManagerResponseDto
            {
                Name = manager.Name,
                LastName = manager.LastName,
                Dni = manager.Dni,
                PhoneNumber = manager.PhoneNumber,
                Email = manager.Email,

                Tournaments = manager.Tournaments?.Select(t => t.ToResponseDto(0)).ToList() ?? new()
            };
        }

        public static IEnumerable<ManagerResponseDto> ToResponseDto(this IEnumerable<Manager> managers)
        {
            return managers.Select(t => t.ToResponseDto());
        }
    }
}
