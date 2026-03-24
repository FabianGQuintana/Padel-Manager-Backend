using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Stage;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IStageService
    {

        // CRUD BÁSICO

        Task<StageResponseDto> AddNewStageAsync(CreateStageDto dto);

        Task<bool> UpdateStageAsync(Guid id, UpdateStageDto dto);

        Task<bool> SoftDeleteToggleStageAsync(Guid id);


        // LÓGICA DE NEGOCIO
 
        // buscar los jugadores de esta etapa, cruzar sus disponibilidades, y 
        // armar las zonas correspondientes y generar todos los partidos adentro.
        Task<bool> GenerateZonesAndMatchesAutomaticAsync(Guid stageId);


        // LECTURA Y FILTROS

        Task<StageResponseDto?> GetStageByIdAsync(Guid stageId);

        Task<IEnumerable<StageResponseDto>> GetAllStagesAsync();

        // Traer todas las etapas que pertenezcan a una categoría en particular.
        Task<IEnumerable<StageResponseDto>> GetStagesByCategoryIdAsync(Guid categoryId);

        Task<IEnumerable<StageResponseDto>> GetStagesByTypeAsync(StageType type);
    }
}