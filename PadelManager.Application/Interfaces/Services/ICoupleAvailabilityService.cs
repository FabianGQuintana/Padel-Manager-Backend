using PadelManager.Application.DTOs.CoupleAvailability;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ICoupleAvailabilityService
    {
        // CRUD BÁSICO
        Task<CoupleAvailabilityResponseDto> AddNewAvailabilityAsync(CreateCoupleAvailabilityDto dto);
        Task<bool> UpdateAvailabilityAsync(Guid id, UpdateCoupleAvailabilityDto dto);
        Task<bool> SoftDeleteToggleAvailabilityAsync(Guid id);

        // LECTURA
        Task<CoupleAvailabilityResponseDto?> GetAvailabilityByIdAsync(Guid id);
        Task<IEnumerable<CoupleAvailabilityResponseDto>> GetAllAvailabilitiesAsync();
        Task<IEnumerable<CoupleAvailabilityResponseDto>> GetAvailabilitiesByCoupleIdAsync(Guid coupleId);
    }
}