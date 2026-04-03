using System;
using System.Collections.Generic;
using System.Linq;
using PadelManager.Application.DTOs.Stage;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Mappers
{
    public static class StageMapper
    {
        // Este Metodo sirve en forma de respuesta desde lo guardado(DB) hacia el FRONTEND.
        public static StageResponseDto ToResponseDto(this Stage stage, int zonesCount = 0, int matchesCount = 0)
        {
            return new StageResponseDto
            {
                // Sigue esta logica = Id(variable que contiene datos DB) = ...Id(Esta var. Sera la encargada de viajar hacia el front.)
                // y asi con todas las variables.
                Id = stage.Id,
                Name = stage.Name,
                Type = stage.Type,
                Order = stage.Order,
                CategoryId = stage.CategoryId,
                IsActive = stage.DeletedAt == null ? "Activo" : "Inactivo",

                // Datos calculados opcionales
                ZonesCount = zonesCount,
                MatchesCount = matchesCount
            };
        }

        // 2. Colección de Entidades -> Colección de DTOs
        // Se usa cuando hacen un GetAll o traemos una lista de elementos.
        // Es recomendable que siempre este este metodo para listas.
        public static IEnumerable<StageResponseDto> ToResponseDto(this IEnumerable<Stage> stages)
        {
            return stages.Select(s => s.ToResponseDto());
        }

        // Este método "mapea" los cambios del DTO a la Entidad que ya existe
        public static void MapToEntity(this Stage existingEntity, UpdateStageDto dto)
        {
            // Con los strings no hay drama porque son Reference Types
            if (dto.Name != null)
                existingEntity.Name = dto.Name;

            // Tipo de valor (Enum, int?) requiere una comprobación HasValue
            if (dto.Type.HasValue)
                existingEntity.Type = dto.Type.Value;

            if (dto.Order.HasValue)
                existingEntity.Order = dto.Order.Value;

            // Fíjate que acá NO mapeamos el CategoryId, ya que por regla de negocio 
            // no permitimos que una etapa cambie de categoría una vez creada.

            // ESTA PARTE ESTA BIEN PERO ES MAS PROFESIONAL REALIZARLO LUEGO EN UN SERVICIO APARTE
            // CurrentUserService. PARA DEJAR LOS MAPPERS MAS LIMPIOS Y DELEGAR ESA TAREA A UN SERVICE ESPECIFICO
        }

        public static Stage ToEntity(this CreateStageDto dto)
        {
            return new Stage
            {
                Name = dto.Name,
                Type = dto.Type,
                Order = dto.Order,
                CategoryId = dto.CategoryId

                // CreatedBy = Dejaremos luego para otro servicio en especifico estas tareas de AUDITORIAS
            };
        }
    }
}