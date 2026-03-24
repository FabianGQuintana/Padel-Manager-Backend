using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.CoupleAvailability;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class CoupleAvailabilityMapper
    {
        // Este Metodo sirve en forma de respuesta desde lo guardado(DB) hacia el FRONTEND.
        public static CoupleAvailabilityResponseDto ToResponseDto(this CoupleAvailability availability)
        {
            return new CoupleAvailabilityResponseDto
            {
                // Sigue esta logica = Id(variable que contiene datos DB) = ...Id(Esta var. Sera la encargada de viajar hacia el front.)
                Id = availability.Id,
                Day = availability.Day,
                From = availability.From,
                To = availability.To,
                CoupleId = availability.CoupleId
            };
        }

        // Colección de Entidades -> Colección de DTOs
        // Se usa cuando hacen un GetAll o traemos una lista de elementos.
        // Es recomendable que siempre este este metodo para listas.
        public static IEnumerable<CoupleAvailabilityResponseDto> ToResponseDto(this IEnumerable<CoupleAvailability> availabilities)
        {
            return availabilities.Select(a => a.ToResponseDto());
        }

        // Este método "mapea" los cambios del DTO a la Entidad que ya existe
        public static void MapToEntity(this CoupleAvailability existingEntity, UpdateCoupleAvailabilityDto dto)
        {
            // DayOfWeek y TimeOnly son tipos de valor, usamos HasValue y Value.
            if (dto.Day.HasValue)
                existingEntity.Day = dto.Day.Value;

            if (dto.From.HasValue)
                existingEntity.From = dto.From.Value;

            if (dto.To.HasValue)
                existingEntity.To = dto.To.Value;

            // CoupleId NO se modifica en un update.
            // La disponibilidad puede cambiar sus horarios,
            // pero siempre sigue perteneciendo a la misma pareja
            // con la que fue creada.

  
        }

        public static CoupleAvailability ToEntity(this CreateCoupleAvailabilityDto dto)
        {
            return new CoupleAvailability
            {
                Day = dto.Day,
                From = dto.From,
                To = dto.To,
                CoupleId = dto.CoupleId

                
            };
        }
    }
}