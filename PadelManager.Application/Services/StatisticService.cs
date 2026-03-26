using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Statistic;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public StatisticService(
            IStatisticRepository statisticRepo,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _statisticRepo = statisticRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        #region CRUD BÁSICO

        public async Task<StatisticResponseDto> AddNewStatisticAsync(CreateStatisticDto dto)
        {
            var statistic = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            statistic.CreatedBy = user;
            statistic.LastModifiedBy = user;

            var result = await _statisticRepo.AddAsync(statistic);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateStatisticAsync(Guid id, UpdateStatisticDto dto)
        {
            var existingStatistic = await _statisticRepo.GetByIdAsync(id);
            if (existingStatistic == null) return false;

            existingStatistic.MapToEntity(dto);

            existingStatistic.LastModifiedBy = _currentUser.UserName ?? "System";
            existingStatistic.LastModifiedAt = DateTime.UtcNow;

            await _statisticRepo.UpdateAsync(existingStatistic);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleStatisticAsync(Guid id)
        {
            var existingStatistic = await _statisticRepo.GetByIdAsync(id);
            if (existingStatistic == null) return false;

            existingStatistic.LastModifiedBy = _currentUser.UserName ?? "System";
            existingStatistic.LastModifiedAt = DateTime.UtcNow;

            await _statisticRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<StatisticResponseDto?> GetStatisticByIdAsync(Guid id)
        {
            var statistic = await _statisticRepo.GetByIdAsync(id);
            return statistic?.ToResponseDto();
        }

        public async Task<IEnumerable<StatisticResponseDto>> GetStatisticsByZoneIdAsync(Guid zoneId)
        {
            var statistics = await _statisticRepo.GetStatisticsByZoneIdAsync(zoneId);
            return statistics.ToResponseDto();
        }

        public async Task<IEnumerable<StatisticResponseDto>> GetAllStatisticsAsync()
        {
            var statistics = await _statisticRepo.GetAllAsync();
            return statistics.ToResponseDto();
        }

        public async Task<IEnumerable<StatisticResponseDto>> GetStatisticsByCoupleIdAsync(Guid coupleId)
        {
            var statistics = await _statisticRepo.GetStatisticsByCoupleIdAsync(coupleId);
            return statistics.ToResponseDto();
        }

        public async Task<StatisticResponseDto?> GetStatisticByCoupleIdAndZoneIdAsync(Guid coupleId, Guid zoneId)
        {
            var statistic = await _statisticRepo.GetStatisticByCoupleIdAndZoneIdAsync(coupleId, zoneId);
            return statistic?.ToResponseDto();
        }

        #endregion
    }
}