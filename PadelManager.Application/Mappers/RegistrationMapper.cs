using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class RegistrationMapper
    {
        public static RegistrationResponseDto ToResponseDto(this Registration registration, string? coupleNames = null, string? categoryName = null, string? tournamentName = null)
        {
           
            var names = coupleNames ?? (registration.Couple != null
                ? $"{registration.Couple.Player1?.LastName} / {registration.Couple.Player2?.LastName}"
                : null);

            return new RegistrationResponseDto
            {
                Id = registration.Id,
                RegistrationDate = registration.RegistrationDate,

                
                RegistrationTime = new TimeOnly(registration.RegistrationTime.Hour, registration.RegistrationTime.Minute, registration.RegistrationTime.Second),

                CoupleId = registration.CoupleId,
                CategoryId = registration.CategoryId,
                TournamentId = registration.TournamentId,
                IsActive = registration.DeletedAt == null ? "Activo" : "Inactivo",

                CoupleNames = names,
                CategoryName = categoryName ?? registration.Category?.Name,
                TournamentName = tournamentName ?? registration.Tournament?.Name
            };
        }


        public static IEnumerable<RegistrationResponseDto> ToResponseDto(this IEnumerable<Registration> registrations)
        {
            return registrations.Select(r => r.ToResponseDto());
        }

        
        public static void MapToEntity(this Registration existingEntity, UpdateRegistrationDto dto)
        {
            if (dto.CoupleId.HasValue)
                existingEntity.CoupleId = dto.CoupleId.Value;

            if (dto.CategoryId.HasValue)
                existingEntity.CategoryId = dto.CategoryId.Value;

            if (dto.TournamentId.HasValue)
                existingEntity.TournamentId = dto.TournamentId.Value;
        }

        public static Registration ToEntity(this CreateRegistrationDto dto)
        {
            var argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, argentinaTimeZone);

            return new Registration
            {
                CoupleId = dto.CoupleId,
                CategoryId = dto.CategoryId,
                TournamentId = dto.TournamentId,
                TotalAmount = dto.TotalAmount,
                Discount = dto.Discount,
                RegistrationDate = DateOnly.FromDateTime(currentDateTime),
                RegistrationTime = new TimeOnly(currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second)
            };
        }
    }
}