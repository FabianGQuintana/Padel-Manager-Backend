using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.DTOs.Zone;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class ZoneMapper
    {
        public static ZoneResponseDto ToResponseDto (this Zone zone)
        {
            return new ZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                StageId = zone.StageId,
                IsActive = zone.DeletedAt == null ? "Activo" : "Inactivo",

                Couples = zone.Couples?.Select(c => c.ToResponseDto()).ToList() ?? new(),

                Matches = zone.Matches?.Select(m => m.ToResponseDto()).ToList() ?? new(),

                Statistics = zone.Statistics?.Select(s => s.ToResponseDto()).ToList() ?? new()

            };
        }

        public static IEnumerable<ZoneResponseDto> ToResponseDto(this IEnumerable<Zone> zones)
        {
            return zones.Select(t => t.ToResponseDto());
        }

        public static void MapToEntity(this Zone existingZone, UpdateZoneDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                existingZone.Name = dto.Name;
            }
            else
            {
                throw new Exception("El nombre de la zona es incorrecto");
            }
        }


        public static Zone ToEntity(this CreateZoneDto dto)
        {
            return new Zone
            {
                Name = dto.Name,
                StageId = dto.stageId,

                Stage = null!
            };
        }

    }
}
