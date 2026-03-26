using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Match;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(
            IMatchRepository matchRepo,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _matchRepo = matchRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        #region CRUD BÁSICO

        public async Task<MatchResponseDto> AddNewMatchAsync(CreateMatchDto dto)
        {
            var match = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            match.CreatedBy = user;
            match.LastModifiedBy = user;

            var result = await _matchRepo.AddAsync(match);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateMatchAsync(Guid id, UpdateMatchDto dto)
        {
            var existingMatch = await _matchRepo.GetByIdAsync(id);
            if (existingMatch == null) return false;

            existingMatch.MapToEntity(dto);

            existingMatch.LastModifiedBy = _currentUser.UserName ?? "System";
            existingMatch.LastModifiedAt = DateTime.UtcNow;

            await _matchRepo.UpdateAsync(existingMatch);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleMatchAsync(Guid id)
        {
            var existingMatch = await _matchRepo.GetByIdAsync(id);
            if (existingMatch == null) return false;

            existingMatch.LastModifiedBy = _currentUser.UserName ?? "System";
            existingMatch.LastModifiedAt = DateTime.UtcNow;

            await _matchRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> LoadMatchResultAsync(Guid matchId, LoadMatchResultDto dto)
        {
            var match = await _matchRepo.GetByIdAsync(matchId);
            if (match == null) return false;

            match.WinnerCoupleId = dto.WinnerCoupleId;
            match.LoserCoupleId = dto.LoserCoupleId;
            match.Set1_coupleA = dto.Set1_coupleA;
            match.Set1_coupleB = dto.Set1_coupleB;
            match.Set2_coupleA = dto.Set2_coupleA;
            match.Set2_coupleB = dto.Set2_coupleB;
            match.Set3_coupleA = dto.Set3_coupleA;
            match.Set3_coupleB = dto.Set3_coupleB;

            match.StatusType = MatchStatus.Completed;

            match.LastModifiedBy = _currentUser.UserName ?? "System";
            match.LastModifiedAt = DateTime.UtcNow;

            await _matchRepo.UpdateAsync(match);
            return await _unitOfWork.SaveChangesAsync() > 0;
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