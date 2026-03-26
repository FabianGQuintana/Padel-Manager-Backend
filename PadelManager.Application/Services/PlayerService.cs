using PadelManager.Application.DTOs.Player;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IPlayerRepository playerRepo, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _playerRepo = playerRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<PlayerResponseDto> AddNewPlayerAsync(CreatePlayerDto dto)
        {
            var player = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            player.CreatedBy = user;
            player.LastModifiedBy = user;

            var result = await _playerRepo.AddAsync(player);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdatePlayerAsync(Guid id, UpdatePlayerDto dto)
        {
            var existingPlayer = await _playerRepo.GetByIdAsync(id);
            if (existingPlayer == null) return false;

            existingPlayer.MapToEntity(dto);

            existingPlayer.LastModifiedBy = _currentUser.UserName ?? "System";
            existingPlayer.LastModifiedAt = DateTime.UtcNow;

            await _playerRepo.UpdateAsync(existingPlayer);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteTogglePlayerAsync(Guid id)
        {
            var existingPlayer = await _playerRepo.GetByIdAsync(id);
            if (existingPlayer == null) return false;

            existingPlayer.LastModifiedBy = _currentUser.UserName ?? "System";
            existingPlayer.LastModifiedAt = DateTime.UtcNow;

            await _playerRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<PlayerResponseDto?> GetPlayerByIdAsync(Guid playerId)
        {
            var player = await _playerRepo.GetByIdAsync(playerId);
            return player?.ToResponseDto();
        }

        public async Task<IEnumerable<PlayerResponseDto>> GetPlayerByNameAsync(string name)
        {
            var players = await _playerRepo.GetPlayerByNameAsync(name);
            return players.ToResponseDto();
        }

        public async Task<IEnumerable<PlayerResponseDto>> GetPlayerByLastNameAsync(string lastName)
        {
            var players = await _playerRepo.GetPlayerByLastNameAsync(lastName);
            return players.ToResponseDto();
        }

        public async Task<PlayerResponseDto?> GetPlayerByPhoneNumberAsync(string phoneNumber)
        {
            var player = await _playerRepo.GetPlayerByPhoneNumberAsync(phoneNumber);
            return player?.ToResponseDto();
        }

        public async Task<PlayerResponseDto?> GetPlayerByDniAsync(string dni)
        {
            var player = await _playerRepo.GetPlayerByDniAsync(dni);
            return player?.ToResponseDto();
        }

        public async Task<IEnumerable<PlayerResponseDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepo.GetAllAsync();
            return players.ToResponseDto();
        }

        public async Task<IEnumerable<PlayerResponseDto>> GetPlayersByAvailabilityAsync(string availability)
        {
            var players = await _playerRepo.GetPlayersByAvailabilityAsync(availability);
            return players.ToResponseDto();
        }

        public async Task<IEnumerable<PlayerResponseDto>> GetPlayersByAgeAsync(byte age)
        {
            var players = await _playerRepo.GetPlayerByAgeAsync(age);
            return players.ToResponseDto();
        }
    }
}