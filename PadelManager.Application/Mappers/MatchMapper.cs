using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PadelManager.Application.DTOs.Match;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Mappers
{
    public static class MatchMapper
    {
        // Este Metodo sirve en forma de respuesta desde lo guardado(DB) hacia el FRONTEND de rucula.
        public static MatchResponseDto ToResponseDto(this Match match)
        {
            return new MatchResponseDto
            {
                // Sigue esta logica = Id(variable que contiene datos DB) = ...Id(Esta var. Sera la encargada de viajar hacia el front.)
                Id = match.Id,
                WinnerCoupleId = match.WinnerCoupleId,
                LoserCoupleId = match.LoserCoupleId,
                DateTime = match.DateTime,
                Status = match.StatusType,
                LocationName = match.LocationName,
                CourtName = match.CourtName,

                // Marcadores
                Set1_coupleA = match.Set1_coupleA,
                Set1_coupleB = match.Set1_coupleB,
                Set2_coupleA = match.Set2_coupleA,
                Set2_coupleB = match.Set2_coupleB,
                Set3_coupleA = match.Set3_coupleA,
                Set3_coupleB = match.Set3_coupleB,
                IsActive = match.DeletedAt == null ? "Activo" : "Inactivo",
                // Relaciones
                StageId = match.StageId,
                ZoneId = match.ZoneId,
                CoupleId = match.CoupleId,
                CoupleId2 = match.CoupleId2
            };
        }


        // 2. Colección de Entidades -> Colección de DTOs
        // Se usa cuando hacen un GetAll o traemos una lista de elementos.
        // Es recomendable que siempre este este metodo para listas.
        public static IEnumerable<MatchResponseDto> ToResponseDto(this IEnumerable<Match> matches)
        {
            return matches.Select(m => m.ToResponseDto());
        }

        // 3. Este método "mapea" los cambios del DTO a la Entidad que ya existe (Este es para editar anashe)
        public static void MapToEntity(this Match existingEntity, UpdateMatchDto dto)
        {
            // Con los strings no hay drama porque son Reference Types
            if (dto.LocationName != null) existingEntity.LocationName = dto.LocationName;
            if (dto.CourtName != null) existingEntity.CourtName = dto.CourtName;

            // Con Guid, DateTime, Enum e Int, usamos .Value (o HasValue) porque son tipos de valor.
            if (dto.WinnerCoupleId.HasValue) existingEntity.WinnerCoupleId = dto.WinnerCoupleId.Value;
            if (dto.LoserCoupleId.HasValue) existingEntity.LoserCoupleId = dto.LoserCoupleId.Value;

            if (dto.DateTime.HasValue) existingEntity.DateTime = dto.DateTime.Value;
            if (dto.Status.HasValue) existingEntity.StatusType = dto.Status.Value;

            // Mapeo de los resultados de los sets
            if (dto.Set1_coupleA.HasValue) existingEntity.Set1_coupleA = dto.Set1_coupleA.Value;
            if (dto.Set1_coupleB.HasValue) existingEntity.Set1_coupleB = dto.Set1_coupleB.Value;
            if (dto.Set2_coupleA.HasValue) existingEntity.Set2_coupleA = dto.Set2_coupleA.Value;
            if (dto.Set2_coupleB.HasValue) existingEntity.Set2_coupleB = dto.Set2_coupleB.Value;

            // Los sets 3 ya son opcionales en la DB, pero los tratamos igual en el Update
            if (dto.Set3_coupleA.HasValue) existingEntity.Set3_coupleA = dto.Set3_coupleA.Value;
            if (dto.Set3_coupleB.HasValue) existingEntity.Set3_coupleB = dto.Set3_coupleB.Value;

            // ESTA PARTE ESTA BIEN PERO ES MAS PROFESIONAL REALIZARLO LUEGO EN UN SERVICIO APARTE
            // CurrentUserService. PARA DEJAR LOS MAPPERS MAS LIMPIOS Y DELEGAR ESA TAREA A UN SERVICE ESPECIFICO
        }

        public static Match ToEntity(this CreateMatchDto dto)
        {
            return new Match
            {
                DateTime = dto.DateTime,
                LocationName = dto.LocationName,
                CourtName = dto.CourtName,
                StageId = dto.StageId,
                ZoneId = dto.ZoneId,
                CoupleId = dto.CoupleId,
                CoupleId2 = dto.CoupleId2,

                // Valores iniciales por defecto que la DB necesita al crear el partido
                StatusType = MatchStatus.Pending,
                WinnerCoupleId = null,
                LoserCoupleId = null,
                Set1_coupleA = 0,
                Set1_coupleB = 0,
                Set2_coupleA = 0,
                Set2_coupleB = 0,
                Set3_coupleA = null,
                Set3_coupleB = null

                // CreatedBy = Dejaremos luego para otro servicio en especifico estas tareas de AUDITORIAS
            };
        }
    }
}