using PadelManager.Application.DTOs.Court;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ICourtService
    {
        // CRUD
        Task<CourtResponseDto> AddNewCourtAsync(CreateCourtDto dto);
        Task<bool> UpdateCourtAsync(Guid id, UpdateCourtDto dto);
        Task<bool> SoftDeleteToggleCourtAsync(Guid id);

        // GETS
        Task<CourtResponseDto?> GetCourtByIdAsync(Guid id);
        Task<IEnumerable<CourtResponseDto>> GetAllCourtsAsync();
        Task<IEnumerable<CourtResponseDto>> GetCourtsByVenueIdAsync(Guid venueId);
        Task<IEnumerable<CourtResponseDto>> GetCourtsByNameAsync(string name);
        Task<IEnumerable<CourtResponseDto>> GetCourtsBySurfaceTypeAsync(string surfaceType);
        Task<IEnumerable<CourtResponseDto>> GetCourtsByAvailabilityAsync(CourtAvailabilityType availability);
    }
}