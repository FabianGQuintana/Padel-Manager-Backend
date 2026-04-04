using System;
using PadelManager.Application.DTOs.Tournament;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Mappers

{
    public static class TournamentMapper
    {
        // Este Metodo sirve en forma de respuesta desde lo guardado(DB) hacia el FRONTEND.
        public static TournamentResponseDto ToResponseDto (this Tournament tournament,int currentRegistrations = 0)
        {
            return new TournamentResponseDto
            {
                // Sigue esta logica = Id(variable que contiene datos DB) = ...Id(Esta var. Sera la encargada de viajar hacia el front.)
                //y asi con todos las variables.
                Id = tournament.Id,
                Name = tournament.Name,
                StartDate = tournament.StartDate,
                Regulations = tournament.Regulations,
                Status = tournament.StatusType.ToString(),
                TournamentType = tournament.TournamentType,
                ManagerId = tournament.ManagerId,
                IsActive = tournament.DeletedAt == null ? "Activo" : "Inactivo",
                ManagerName = tournament.Managers != null && tournament.Managers.Any()
                ? string.Join(", ", tournament.Managers.Select(m => $"{m.User?.Name} {m.User?.LastName}"))
                : "Sin Organizador"

            };
        }

        // 2. Colección de Entidades -> Colección de DTOs
        // Se usa cuando hacen un GetAll o traemos una lista de elementos (por ejemplo, todos los torneos de una categoría).
        //Es recomendable que siempre este este metodo. para listas.
        public static IEnumerable<TournamentResponseDto> ToResponseDto(this IEnumerable<Tournament> tournaments)
        {
            return tournaments.Select(t => t.ToResponseDto());
        }

        //Este método "mapea" los cambios del DTO a la Entidad que ya existe
        public static void MapToEntity(this Tournament existingEntity, UpdateTournamentDto dto )
        {
            // Con los strings no hay drama porque son Reference Types
            if (dto.Name != null) existingEntity.Name = dto.Name;
            if (dto.Regulations != null) existingEntity.Regulations = dto.Regulations;
            if (dto.TournamentType != null) existingEntity.TournamentType = dto.TournamentType;

            // Con Guid, DateTime e Int, usamos .Value por que son tipos de valor.
            if (dto.StartDate != null)
                existingEntity.StartDate = dto.StartDate.Value;


            if (dto.ManagerId != null)
                existingEntity.ManagerId = dto.ManagerId.Value;
        }


        public static Tournament ToEntity (this CreateTournamentDto dto)
        {
            return new Tournament
            {
                Name = dto.Name,
                Regulations = dto.Regulations,
                StartDate = dto.StartDate,
                TournamentType = dto.TournamentType,
                ManagerId = dto.ManagerId,
                StatusType = TournamentStatus.Draft
                
               // CreatedBy = Dejaremos luego para otro servicio en especifico estas tareas de AUDITORIAS
            };
        }

    }
}
