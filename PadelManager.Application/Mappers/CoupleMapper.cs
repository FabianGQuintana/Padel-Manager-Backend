using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Couple;
using PadelManager.Domain.Entities;
using PadelManager.Application.DTOs.CoupleAvailability;

namespace PadelManager.Application.Mappers
{
    public static class CoupleMapper
    {
        // Entidad -> Response DTO
        public static CoupleResponseDto ToResponseDto(this Couple couple)
        {
            return new CoupleResponseDto
            {
                Id = couple.Id,
                Nickname = couple.Nickname,

                Player1Id = couple.Player1Id,
                Player1Name = couple.Player1 != null
                    ? $"{couple.Player1.Name} {couple.Player1.LastName}"
                    : string.Empty,

                Player2Id = couple.Player2Id,
                Player2Name = couple.Player2 != null
                    ? $"{couple.Player2.Name} {couple.Player2.LastName}"
                    : string.Empty,


                IsActive = couple.DeletedAt == null ? "Activo" : "Inactivo",

                Availabilities = couple.Availabilities
                    .Select(a => new CoupleAvailabilityResponseDto
                    {
                        Id = a.Id,
                        Day = a.Day,
                        From = a.From,
                        To = a.To
                    })
                    .ToList()
            };
        }

        // Colección de entidades -> colección de Response DTOs
        public static IEnumerable<CoupleResponseDto> ToResponseDto(this IEnumerable<Couple> couples)
        {
            return couples.Select(c => c.ToResponseDto());
        }

        // Create DTO -> Entidad
        public static Couple ToEntity(this CreateCoupleDto dto)
        {
            return new Couple
            {
                Nickname = dto.Nickname,
                Player1Id = dto.Player1Id,
                Player2Id = dto.Player2Id,
                // Reutilizamos el método de arriba de forma limpia
                Availabilities = dto.Availabilities != null
                    ? dto.Availabilities.Select(a => a.ToEntity()).ToList()//Si Availabilities trae datos, hace el mapeo
                    : new List<CoupleAvailability>()// Si el DTO no trae Availabilities, se asigna una lista vacía para evitar problemas de nullabilidad.
            };
        }

        // Update DTO -> actualiza entidad existente
        public static void MapToEntity(this UpdateCoupleDto dto, Couple couple, bool updateAvailabilities = true)
        {
            couple.Nickname = dto.Nickname;
            couple.Player1Id = dto.Player1Id;
            couple.Player2Id = dto.Player2Id;

            if (updateAvailabilities && dto.Availabilities != null)
            {
                couple.Availabilities = dto.Availabilities
                    .Where(a => a.Day.HasValue && a.From.HasValue && a.To.HasValue) // Solo si están los 3 datos
                    .Select(a => new CoupleAvailability
                    {
                        Day = (DayOfWeek)a.Day!.Value, // El ! le dice: "Ya sé que no es nulo por el Where"
                        From = a.From!.Value,
                        To = a.To!.Value,
                        CoupleId = couple.Id
                    })
                    .ToList();
            }
        }
    }
}