using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Couple;
using PadelManager.Domain.Entities;

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

                ZoneId = couple.ZoneId,
                ZoneName = couple.Zone != null
                    ? couple.Zone.Name
                    : null,

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
                Availabilities = dto.Availabilities
                    .Select(a => new CoupleAvailability
                    {
                        Day = a.Day,
                        From = a.From,
                        To = a.To
                    })
                    .ToList()
            };
        }

        // Update DTO -> actualiza entidad existente
        public static void MapToEntity(this UpdateCoupleDto dto, Couple couple, bool updateAvailabilities = true)
        {
            couple.Nickname = dto.Nickname;
            couple.Player1Id = dto.Player1Id;
            couple.Player2Id = dto.Player2Id;

            if (updateAvailabilities)
            {
                couple.Availabilities = dto.Availabilities
                    .Select(a => new CoupleAvailability
                    {
                        Day = a.Day,
                        From = a.From,
                        To = a.To,
                        CoupleId = couple.Id
                    })
                    .ToList();
            }
        }
    }
}