using PadelManager.Application.DTOs.Venue;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IVenueService
    {
        // CRUD
        Task<VenueResponseDto> AddNewVenueAsync(CreateVenueDto dto);
        Task<bool> UpdateVenueAsync(Guid id, UpdateVenueDto dto);
        Task<bool> SoftDeleteToggleVenueAsync(Guid id);

        // GETs
        Task<VenueResponseDto?> GetVenueByIdAsync(Guid id);
        Task<IEnumerable<VenueResponseDto>> GetAllVenuesAsync();
        Task<IEnumerable<VenueResponseDto>> GetVenuesByCityAsync(string city);
        Task<IEnumerable<VenueResponseDto>> GetVenuesByNameAsync(string name);
        Task<VenueResponseDto?> GetVenueByAddressAsync(string address);
    }
}