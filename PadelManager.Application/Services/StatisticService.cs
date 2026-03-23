using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Statistic;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepo;

        public StatisticService(IStatisticRepository statisticRepo)
        {
            _statisticRepo = statisticRepo;
        }

        #region CRUD BÁSICO

        public async Task<StatisticResponseDto> AddNewStatisticAsync(CreateStatisticDto dto)
        {
            var statistic = dto.ToEntity();
            var result = await _statisticRepo.AddAsync(statistic);

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateStatisticAsync(Guid id, UpdateStatisticDto dto)
        {
            var existingStatistic = await _statisticRepo.GetByIdAsync(id);
            if (existingStatistic == null) return false;

            existingStatistic.MapToEntity(dto);
            await _statisticRepo.UpdateAsync(existingStatistic);
            return true;
        }

        public async Task<bool> SoftDeleteToggleStatisticAsync(Guid id)
        {
            var result = await _statisticRepo.SoftDeleteToggleAsync(id);
            return result != null;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<StatisticResponseDto?> GetStatisticByIdAsync(Guid id)
        {
            var statistic = await _statisticRepo.GetByIdAsync(id);
            if (statistic == null) return null;

            return statistic.ToResponseDto();
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