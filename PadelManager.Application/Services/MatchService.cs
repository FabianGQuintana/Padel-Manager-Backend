using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Match;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepo;

        public MatchService(IMatchRepository matchRepo)
        {
            _matchRepo = matchRepo;
        }

        #region CRUD BÁSICO

        public async Task<MatchResponseDto> AddNewMatchAsync(CreateMatchDto dto)
        {
            var match = dto.ToEntity();
            var result = await _matchRepo.AddAsync(match);
            return result.ToResponseDto();
        }

        public async Task<bool> UpdateMatchAsync(Guid id, UpdateMatchDto dto)
        {
            var existingMatch = await _matchRepo.GetByIdAsync(id);
            if (existingMatch == null) return false;

            existingMatch.MapToEntity(dto);
            await _matchRepo.UpdateAsync(existingMatch);
            return true;
        }

        public async Task<bool> SoftDeleteToggleMatchAsync(Guid id)
        {
            var result = await _matchRepo.SoftDeleteToggleAsync(id);
            return result != null;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> LoadMatchResultAsync(Guid matchId, LoadMatchResultDto dto)
        {
            var match = await _matchRepo.GetByIdAsync(matchId);
            if (match == null) return false;

            // Actualizamos solo los datos del resultado
            match.WinnerCoupleId = dto.WinnerCoupleId;
            match.LoserCoupleId = dto.LoserCoupleId;
            match.Set1_coupleA = dto.Set1_coupleA;
            match.Set1_coupleB = dto.Set1_coupleB;
            match.Set2_coupleA = dto.Set2_coupleA;
            match.Set2_coupleB = dto.Set2_coupleB;
            match.Set3_coupleA = dto.Set3_coupleA;
            match.Set3_coupleB = dto.Set3_coupleB;

            // Regla: el partido finaliza automáticamente
            match.StatusType = MatchStatus.Completed;

            await _matchRepo.UpdateAsync(match);
            return true;
        }

        #endregion

        #region LECTURA BÁSICA Y DETALLES

        public async Task<MatchResponseDto?> GetMatchByIdAsync(Guid matchId)
        {
            var match = await _matchRepo.GetByIdAsync(matchId);
            return match?.ToResponseDto();
        }

        public async Task<MatchResponseDto?> GetMatchWithDetailsByIdAsync(Guid matchId)
        {
            var match = await _matchRepo.GetMatchWithDetailsByIdAsync(matchId);
            return match?.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetAllMatchesAsync()
        {
            var matches = await _matchRepo.GetAllAsync();
            return matches.ToResponseDto();
        }

        #endregion

        #region FILTROS Y BÚSQUEDAS ESPECÍFICAS

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByDateAsync(DateTime date)
        {
            var matches = await _matchRepo.GetMatchesByDateAsync(date);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByLocationAsync(string locationName)
        {
            var matches = await _matchRepo.GetMatchesByLocationAsync(locationName);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByCourtAsync(string courtName)
        {
            var matches = await _matchRepo.GetMatchesByCourtAsync(courtName);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByStatusAsync(MatchStatus status)
        {
            var matches = await _matchRepo.GetMatchesByStatusAsync(status);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByStageIdAsync(Guid stageId)
        {
            var matches = await _matchRepo.GetMatchesByStageIdAsync(stageId);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByZoneIdAsync(Guid zoneId)
        {
            var matches = await _matchRepo.GetMatchesByZoneIdAsync(zoneId);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByCoupleIdAsync(Guid coupleId)
        {
            var matches = await _matchRepo.GetMatchesByCoupleIdAsync(coupleId);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByWinnerAsync(Guid coupleId)
        {
            var matches = await _matchRepo.GetMatchesByWinnerAsync(coupleId);
            return matches.ToResponseDto();
        }

        public async Task<IEnumerable<MatchResponseDto>> GetMatchesByPlayerIdAsync(Guid playerId)
        {
            var matches = await _matchRepo.GetMatchesByPlayerIdAsync(playerId);
            return matches.ToResponseDto();
        }

        #endregion
    }
}