using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Statistic;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IStatisticService
    {
        // CRUD BÁSICO
        Task<StatisticResponseDto> AddNewStatisticAsync(CreateStatisticDto dto);

        Task<bool> UpdateStatisticAsync(Guid id, UpdateStatisticDto dto);

        Task<bool> SoftDeleteToggleStatisticAsync(Guid id);

        // LECTURA
        Task<StatisticResponseDto?> GetStatisticByIdAsync(Guid id);

        Task<IEnumerable<StatisticResponseDto>> GetStatisticsByZoneIdAsync(Guid zoneId);

        Task<IEnumerable<StatisticResponseDto>> GetStatisticsByCoupleIdAsync(Guid coupleId);

        Task<StatisticResponseDto?> GetStatisticByCoupleIdAndZoneIdAsync(Guid coupleId, Guid zoneId);
    }
}