using PadelManager.Application.DTOs.Manager;
using PadelManager.Domain.Entities;

public static class ManagerMapper
{
    public static ManagerResponseDto ToResponseDto(this Manager entity)
    {
        
        return new ManagerResponseDto
        {
            Id = entity.Id,
            FullName = $"{entity.User.Name} {entity.User.LastName}",
            Email = entity.User.Email,
            Dni = entity.User.Dni,
            YearExperience = entity.YearExperience,
            LicenceAPA = entity.LicenceAPA,
            RoleName = entity.User.Role?.NameRol.ToString() ?? "Sin Rol"
        };
    }

    public static void UpdateEntity(this Manager entity, UpdateManagerDto dto)
    {
        
        entity.YearExperience = dto.YearExperience;
        entity.LicenceAPA = dto.LicenceAPA;

        entity.User.Name = dto.Name;
        entity.User.LastName = dto.LastName;
        entity.User.PhoneNumber = dto.PhoneNumber;
        entity.User.LastModifiedAt = DateTime.UtcNow;
    }
}