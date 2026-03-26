using PadelManager.Application.DTOs.Player;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo;

        public PlayerService(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        public async Task<PlayerResponseDto> AddNewPlayerAsync(CreatePlayerDto dto)
        {
            var player = dto.ToEntity();
            var result = await _playerRepo.AddAsync(player);

            return result.ToResponseDto();
        }

        public async Task<bool> UpdatePlayerAsync(Guid id, UpdatePlayerDto dto)
        {
            var existingPlayer = await _playerRepo.GetByIdAsync(id);
            if (existingPlayer == null) return false;

            existingPlayer.MapToEntity(dto);
            await _playerRepo.UpdateAsync(existingPlayer);

            return true;
        }

        public async Task<bool> SoftDeleteTogglePlayerAsync(Guid id)
        {
            var result = await _playerRepo.SoftDeleteToggleAsync(id);

            return result != null;
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

        public async Task<IEnumerable<PlayerResponseDto>> GetPlayersByAgeAsync(Byte age)
        {
            var players = await _playerRepo.GetPlayerByAgeAsync(age);

            return players.ToResponseDto();
        }

    
    }
}