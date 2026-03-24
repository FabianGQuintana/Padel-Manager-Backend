using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Stage;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class StageService : IStageService
    {
        private readonly IStageRepository _stageRepo;
        private readonly IZoneRepository _zoneRepo;
        private readonly IMatchRepository _matchRepo;

        // Inyectamos los repositorios necesarios para poder contar zonas y partidos, 
        // y más adelante armar el algoritmo automático.
        public StageService(IStageRepository stageRepo, IZoneRepository zoneRepo, IMatchRepository matchRepo)
        {
            _stageRepo = stageRepo;
            _zoneRepo = zoneRepo;
            _matchRepo = matchRepo;
        }

        #region CRUD BÁSICO

        public async Task<StageResponseDto> AddNewStageAsync(CreateStageDto dto)
        {
            var stage = dto.ToEntity();
            var result = await _stageRepo.AddAsync(stage);

            // Al ser nueva, arranca con 0 zonas y 0 partidos
            return result.ToResponseDto(0, 0);
        }

        public async Task<bool> UpdateStageAsync(Guid id, UpdateStageDto dto)
        {
            var existingStage = await _stageRepo.GetByIdAsync(id);
            if (existingStage == null) return false;

            existingStage.MapToEntity(dto);
            await _stageRepo.UpdateAsync(existingStage);

            return true;
        }

        public async Task<bool> SoftDeleteToggleStageAsync(Guid id)
        {
            var result = await _stageRepo.SoftDeleteToggleAsync(id);
            return result != null;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> GenerateZonesAndMatchesAutomaticAsync(Guid stageId)
        {
            var stage = await _stageRepo.GetByIdAsync(stageId);
            if (stage == null) return false;

            // =========================================================================
            // TODO: ALGORITMO DE SCHEDULING (Emparejamiento por disponibilidad)
            // =========================================================================
            // Esta es la parte "inteligente" del sistema. Los pasos lógicos acá serán:
            // 1. Ir a buscar las parejas inscriptas en stage.CategoryId
            // 2. Leer los horarios libres de cada pareja
            // 3. Agrupar las que coinciden
            // 4. Crear las entidades Zone (Zona A, Zona B, etc.) con await _zoneRepo.AddAsync()
            // 5. Crear los partidos cruzándolos y asignarles fecha/hora con await _matchRepo.AddAsync()
            // =========================================================================

            // Retornamos true simulando que el algoritmo corrió bien por ahora
            return true;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<StageResponseDto?> GetStageByIdAsync(Guid stageId)
        {
            var stage = await _stageRepo.GetByIdAsync(stageId);
            if (stage == null) return null;

            // Buscamos las cantidades reales para mandarle al Frontend datos ricos
            var zonesCount = await _zoneRepo.CountByStageIdAsync(stageId);
            var matchesCount = await _matchRepo.CountByStageIdAsync(stageId);

            return stage.ToResponseDto(zonesCount, matchesCount);
        }

        public async Task<IEnumerable<StageResponseDto>> GetAllStagesAsync()
        {
            var stages = await _stageRepo.GetAllAsync();
            // Para listas generales, el Mapper que hicimos asume 0 por defecto 
            // (para no matar a la base de datos haciendo un Count() por cada fila)
            return stages.ToResponseDto();
        }

        public async Task<IEnumerable<StageResponseDto>> GetStagesByCategoryIdAsync(Guid categoryId)
        {
            var stages = await _stageRepo.GetStagesByCategoryIdAsync(categoryId);
            return stages.ToResponseDto();
        }

        public async Task<IEnumerable<StageResponseDto>> GetStagesByTypeAsync(StageType type)
        {
            var stages = await _stageRepo.GetStagesByTypeAsync(type);
            return stages.ToResponseDto();
        }

        #endregion
    }
}