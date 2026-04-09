using PadelManager.Application.DTOs.Venue;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;

namespace PadelManager.Application.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public VenueService(IVenueRepository venueRepo, IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _venueRepo = venueRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<VenueResponseDto> AddNewVenueAsync(CreateVenueDto dto)
        {
            var venue = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";
            venue.CreatedBy = user;
            venue.LastModifiedBy = user;

            await _venueRepo.AddAsync(venue);
            await _unitOfWork.SaveChangesAsync();
            return venue.ToResponseDto();
        }

        public async Task<bool> UpdateVenueAsync(Guid id, UpdateVenueDto dto)
        {
            var existing = await _venueRepo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.MapToEntity(dto);
            existing.LastModifiedBy = _currentUser.UserName ?? "System";
            existing.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleVenueAsync(Guid id)
        {
            var venue = await _venueRepo.GetByIdAsync(id);
            if (venue == null) return false;

            venue.LastModifiedBy = _currentUser.UserName ?? "System";
            venue.LastModifiedAt = DateTime.UtcNow;

            await _venueRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<VenueResponseDto?> GetVenueByIdAsync(Guid id) =>
            (await _venueRepo.GetByIdAsync(id))?.ToResponseDto();

        public async Task<IEnumerable<VenueResponseDto>> GetAllVenuesAsync() =>
            (await _venueRepo.GetAllAsync()).ToResponseDto();

        public async Task<IEnumerable<VenueResponseDto>> GetVenuesByCityAsync(string city) =>
            (await _venueRepo.GetVenuesByCityAsync(city)).ToResponseDto();

        public async Task<IEnumerable<VenueResponseDto>> GetVenuesByNameAsync(string name)
        {
            // El repositorio debe manejar la lógica de búsqueda (ej. usando .Contains())
            var venues = await _venueRepo.GetVenueByNameAsync(name);
            return venues.ToResponseDto();
        }

        public async Task<VenueResponseDto?> GetVenueByAddressAsync(string address)
        {
            var venue = await _venueRepo.GetVenueByAddressAsync(address);

            if (venue == null) return null;

            return venue.ToResponseDto();
        }

    }
}