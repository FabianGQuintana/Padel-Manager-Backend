using PadelManager.Application.DTOs.Player;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerResponseDto ToResponseDto(this Player player)
        {
            return new PlayerResponseDto
            {
                Id = player.Id,
                Name = player.Name,
                LastName = player.LastName,
                PhoneNumber = player.PhoneNumber,
                Dni = player.Dni,
                Age = player.Age,
                IsActive = player.DeletedAt == null ? "Activo" : "Inactivo",
                Availability = player.Availability ?? string.Empty
            };
        }

        public static IEnumerable<PlayerResponseDto> ToResponseDto(this IEnumerable<Player> players)
        {
            return players.Select(p => p.ToResponseDto());
        }

        public static void MapToEntity(this Player existingEntity, UpdatePlayerDto dto)
        {
            if (dto.Name != null) existingEntity.Name = dto.Name.Trim();
            if (dto.LastName != null) existingEntity.LastName = dto.LastName.Trim();
            if (dto.PhoneNumber != null) existingEntity.PhoneNumber = dto.PhoneNumber;
            if (dto.Dni != null) existingEntity.Dni = dto.Dni;
            if (dto.Availability != null) existingEntity.Availability = dto.Availability;
            if (dto.Age.HasValue)
                existingEntity.Age = dto.Age.Value;
        }

        public static Player ToEntity(this CreatePlayerDto dto)
        {
            return new Player
            {
                Name = dto.Name,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Dni = dto.Dni,
                Age = dto.Age,
                Availability = dto.Availability
            };
        }
    }
}