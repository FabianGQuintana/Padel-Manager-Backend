using PadelManager.Application.DTOs.Sanction;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    namespace PadelManager.Application.Mappers
    {
        public static class SanctionMapper
        {
            public static SanctionResponseDto ToResponseDto(this Sanction sanction)
            {
                return new SanctionResponseDto
                {
                    Id = sanction.Id,
                    Reason = sanction.Reason,
                    Severity = sanction.Severity.ToString(),
                    ExpirationDate = sanction.ExpirationDate,
                    PlayerId = sanction.PlayerId,
                    IsActive = (sanction.Severity == StatusSeverity.Alto)
            ? "Blacklist"
            : (sanction.DeletedAt == null &&
              (!sanction.ExpirationDate.HasValue || sanction.ExpirationDate > DateTime.UtcNow))
              ? "Vigente" : "Inactiva"
                };
            }

            public static IEnumerable<SanctionResponseDto> ToResponseDto(this IEnumerable<Sanction> sanctions)
            {
                return sanctions.Select(s => s.ToResponseDto());
            }

            public static void MapToEntity(this Sanction existingEntity, UpdateSanctionDto dto)
            {
                if (!string.IsNullOrEmpty(dto.Reason)) existingEntity.Reason = dto.Reason;
                if (dto.ExpirationDate.HasValue) existingEntity.ExpirationDate = dto.ExpirationDate.Value;
            }

            public static Sanction ToEntity(this CreateSanctionDto dto)
            {
                return new Sanction
                {
                    Reason = dto.Reason,
                    PlayerId = dto.PlayerId,
                    ExpirationDate = dto.ExpirationDate,
                    Severity = dto.Severity
                };
            }
        }
    }
}
