using PadelManager.Application.DTOs.Role;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    public static class RoleMapper
    {
        public static RoleResponseDto ToDto(this Role entity)
        {
            if (entity == null) return null!;

            return new RoleResponseDto
            {
                Id = entity.Id,
                IsActive = entity.DeletedAt == null ? "Activo" : "Inactivo",
                Name = entity.NameRol.ToString()
            };
        }
    }
}
