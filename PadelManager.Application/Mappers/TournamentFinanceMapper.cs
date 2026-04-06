using PadelManager.Application.DTOs.TournamentFinance;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    namespace PadelManager.Application.Mappers
    {
        public static class TournamentFinanceMapper
        {
            public static TournamentFinanceResponseDto ToResponseDto(this TournamentFinance finance)
            {
                return new TournamentFinanceResponseDto
                {
                    Id = finance.Id,
                    FinanceConcept = finance.FinanceConcept,
                    Amount = finance.Amount,
                    MovementType = finance.MovementType.ToString(), // "Income" o "Expense"
                    DateMovement = finance.DateMovement,
                    TournamentId = finance.TournamentId
                };
            }

            public static IEnumerable<TournamentFinanceResponseDto> ToResponseDto(this IEnumerable<TournamentFinance> finances)
            {
                return finances.Select(f => f.ToResponseDto());
            }

            public static void MapToEntity(this TournamentFinance existingEntity, UpdateTournamentFinanceDto dto)
            {
                if (!string.IsNullOrEmpty(dto.FinanceConcept)) existingEntity.FinanceConcept = dto.FinanceConcept;
                if (dto.Amount.HasValue) existingEntity.Amount = dto.Amount.Value;
                if (dto.MovementType.HasValue) existingEntity.MovementType = dto.MovementType.Value;
            }

            public static TournamentFinance ToEntity(this CreateTournamentFinanceDto dto)
            {
                return new TournamentFinance
                {
                    FinanceConcept = dto.FinanceConcept,
                    Amount = dto.Amount,
                    MovementType = dto.MovementType,
                    TournamentId = dto.TournamentId,
                    DateMovement = DateTime.UtcNow
                };
            }
        }
    }
}
