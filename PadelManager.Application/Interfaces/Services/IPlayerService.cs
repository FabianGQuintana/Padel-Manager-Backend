using PadelManager.Application.DTOs.Player;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IPlayerService
    {
        // CRUD BÁSICO
        Task<PlayerResponseDto> AddNewPlayerAsync(CreatePlayerDto dto);
        Task<bool> UpdatePlayerAsync(Guid id, UpdatePlayerDto dto);
        Task<bool> SoftDeleteTogglePlayerAsync(Guid id);

        // LECTURA
        Task<PlayerResponseDto?> GetPlayerByIdAsync(Guid playerId);
        Task<IEnumerable<PlayerResponseDto>> GetPlayerByNameAsync(string name);
        Task<IEnumerable<PlayerResponseDto>> GetPlayerByLastNameAsync(string lastName); 
        Task<PlayerResponseDto?> GetPlayerByPhoneNumberAsync(string phoneNumber);
        Task<PlayerResponseDto?> GetPlayerByDniAsync(string dni);
        Task<IEnumerable<PlayerResponseDto>> GetAllPlayersAsync();
        Task<IEnumerable<PlayerResponseDto>> GetPlayersByAgeAsync(Byte age);
        Task<IEnumerable<PlayerResponseDto>> GetPlayersByAvailabilityAsync(string availability);
    }
}