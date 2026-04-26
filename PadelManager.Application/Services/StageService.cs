using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Stage;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
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
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public StageService(
            IStageRepository stageRepo,
            IZoneRepository zoneRepo,
            IMatchRepository matchRepo,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _stageRepo = stageRepo;
            _zoneRepo = zoneRepo;
            _matchRepo = matchRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        #region CRUD BÁSICO

        public async Task<StageResponseDto> AddNewStageAsync(CreateStageDto dto)
        {
            var stage = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            stage.CreatedBy = user;
            stage.LastModifiedBy = user;

            var result = await _stageRepo.AddAsync(stage);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto(0, 0);
        }

        public async Task<bool> UpdateStageAsync(Guid id, UpdateStageDto dto)
        {
            var existingStage = await _stageRepo.GetByIdAsync(id);
            if (existingStage == null) return false;

            existingStage.MapToEntity(dto);

            existingStage.LastModifiedBy = _currentUser.UserName ?? "System";
            existingStage.LastModifiedAt = DateTime.UtcNow;

            await _stageRepo.UpdateAsync(existingStage);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleStageAsync(Guid id)
        {
            // 1. Buscamos con hijos
            var stage = await _stageRepo.GetByIdWithChildrenAsync(id);
            if (stage == null) return false;

            // 2. Validación de negocio
            if (!stage.IsDeleted)
            {
                if (stage.Zones.Any(z => !z.IsDeleted))
                    throw new InvalidOperationException("No se puede eliminar la etapa: tiene zonas creadas.");

                if (stage.Matches.Any(m => !m.IsDeleted))
                    throw new InvalidOperationException("No se puede eliminar la etapa: ya tiene partidos generados.");
            }

            stage.LastModifiedBy = _currentUser.UserName ?? "System";
            stage.LastModifiedAt = DateTime.UtcNow;

            await _stageRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> GenerateZonesAndMatchesAutomaticAsync(Guid stageId)
        {
            var stage = await _stageRepo.GetByIdAsync(stageId);
            if (stage == null) return false;

            stage.LastModifiedBy = _currentUser.UserName ?? "System";
            stage.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<StageResponseDto?> GetStageByIdAsync(Guid stageId)
        {
            var stage = await _stageRepo.GetByIdAsync(stageId);
            if (stage == null) return null;

            var zonesCount = await _zoneRepo.CountByStageIdAsync(stageId);
            var matchesCount = await _matchRepo.CountByStageIdAsync(stageId);

            return stage.ToResponseDto(zonesCount, matchesCount);
        }

        public async Task<IEnumerable<StageResponseDto>> GetAllStagesAsync()
        {
            var stages = await _stageRepo.GetAllAsync();
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