using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Mappers
{
    public static class RegistrationMapper
    {
        // Este Metodo sirve en forma de respuesta desde la DB hacia el FRONTEND.
        // Se agregan parámetros opcionales por si el Service quiere mandarle los nombres ya resueltos.
        public static RegistrationResponseDto ToResponseDto(this Registration registration, string? coupleNames = null, string? categoryName = null, string? tournamentName = null)
        {
            return new RegistrationResponseDto
            {
                Id = registration.Id,
                RegistrationDate = registration.RegistrationDate,
                RegistrationTime = registration.RegistrationTime,
                CoupleId = registration.CoupleId,
                CategoryId = registration.CategoryId,
                TournamentId = registration.TournamentId,

                // Si el servicio nos manda los nombres por parámetro, los usamos.
                // Si no, intentamos sacarlos de las propiedades de navegación (si el Repositorio usó .Include())
                CoupleNames = coupleNames,
                CategoryName = categoryName ?? registration.Category?.Name,
                TournamentName = tournamentName ?? registration.Tournament?.Name
            };
        }

        // Colección de Entidades -> Colección de DTOs
        // Se usa cuando hacen un GetAll o traemos una lista de inscriptos.
        public static IEnumerable<RegistrationResponseDto> ToResponseDto(this IEnumerable<Registration> registrations)
        {
            return registrations.Select(r => r.ToResponseDto());
        }

        // Este método "mapea" los cambios del DTO a la Entidad que ya existe
        public static void MapToEntity(this Registration existingEntity, UpdateRegistrationDto dto)
        {
            // Con Guid usamos .HasValue porque en el UpdateDto son opcionales (Guid?)
            if (dto.CoupleId.HasValue)
                existingEntity.CoupleId = dto.CoupleId.Value;

            if (dto.CategoryId.HasValue)
                existingEntity.CategoryId = dto.CategoryId.Value;

            if (dto.TournamentId.HasValue)
                existingEntity.TournamentId = dto.TournamentId.Value;
        }

        public static Registration ToEntity(this CreateRegistrationDto dto)
        {
            // Capturamos el momento exacto en el servidor para evitar trampas desde el Frontend
            var currentDateTime = DateTime.Now;

            return new Registration
            {
                CoupleId = dto.CoupleId,
                CategoryId = dto.CategoryId,
                TournamentId = dto.TournamentId,

                RegistrationDate = DateOnly.FromDateTime(currentDateTime),
                RegistrationTime = TimeOnly.FromDateTime(currentDateTime)

                // CreatedBy = Dejaremos luego para otro servicio en especifico estas tareas de AUDITORIAS
            };
        }
    }
}