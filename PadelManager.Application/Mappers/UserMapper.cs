using PadelManager.Application.DTOs.User;
using PadelManager.Domain.Entities;
using System;

namespace PadelManager.Application.Mappers
{
    public static class UserMapper
    {
        // 🎯 FIX para el error de ToResponseDto
        public static UserResponseDto ToResponseDto(this User entity)
        {
            if (entity == null) return null!;

            return new UserResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                LastName = entity.LastName,
                Dni = entity.Dni,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                // Si la entidad Role está cargada (.Include), mostramos el nombre
                RoleName = entity.Role?.NameRol.ToString() ?? "Sin Rol"
            };
        }

        public static User ToEntity(this CreateUserDto dto)
        {
            if (dto == null) return null!;

            return new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                LastName = dto.LastName,
                Dni = dto.Dni,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId,
                PasswordHash = string.Empty,
                CreatedAt = DateTime.UtcNow,
                Status = "Active"
            };
        }
    }
}