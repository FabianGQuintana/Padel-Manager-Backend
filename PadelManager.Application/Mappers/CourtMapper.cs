using PadelManager.Application.DTOs.Court;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    namespace PadelManager.Application.Mappers
    {
        public static class CourtMapper
        {
            public static CourtResponseDto ToResponseDto(this Court court)
            {
                return new CourtResponseDto
                {
                    Id = court.Id,
                    Name = court.Name,
                    SurfaceType = court.SurfaceType,
                    CourtAvailability = court.CourtAvailability,
                    VenueId = court.VenueId
                };
            }

            public static IEnumerable<CourtResponseDto> ToResponseDto(this IEnumerable<Court> courts)
            {
                return courts.Select(c => c.ToResponseDto());
            }

            public static void MapToEntity(this Court existingEntity, UpdateCourtDto dto)
            {
                if (!string.IsNullOrEmpty(dto.Name)) existingEntity.Name = dto.Name;
                if (!string.IsNullOrEmpty(dto.SurfaceType)) existingEntity.SurfaceType = dto.SurfaceType;
                if (dto.CourtAvailability.HasValue) existingEntity.CourtAvailability = dto.CourtAvailability.Value;
            }

            public static Court ToEntity(this CreateCourtDto dto)
            {
                return new Court
                {
                    Name = dto.Name,
                    SurfaceType = dto.SurfaceType,
                    VenueId = dto.VenueId,
                    CourtAvailability = dto.CourtAvailability
                };
            }
        }
    }
}
