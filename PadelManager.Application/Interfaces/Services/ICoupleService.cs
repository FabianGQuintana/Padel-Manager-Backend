using PadelManager.Application.DTOs.Couple;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ICoupleService
    {
        // CRUD BÁSICO
        Task<CoupleResponseDto> AddNewCoupleAsync(CreateCoupleDto dto);
        Task<bool> UpdateCoupleAsync(Guid id, UpdateCoupleDto dto);
        Task<bool> SoftDeleteToggleCoupleAsync(Guid id);

        // LÓGICA DE NEGOCIO
        Task<bool> ReplacePlayerInCoupleAsync(Guid coupleId, ReplaceCouplePlayerDto dto);

        // LECTURA
        Task<CoupleResponseDto?> GetCoupleByIdAsync(Guid coupleId);
        Task<CoupleResponseDto?> GetCoupleByNicknameAsync(string nickname);
        Task<IEnumerable<CoupleResponseDto>> GetAllCouplesAsync();
        Task<IEnumerable<CoupleResponseDto>> GetCouplesByPlayerIdAsync(Guid playerId);
        Task<IEnumerable<CoupleResponseDto>> GetCouplesByZoneIdAsync(Guid zoneId);
        Task<IEnumerable<CoupleResponseDto>> GetCouplesWithoutZoneAsync();
    }
}