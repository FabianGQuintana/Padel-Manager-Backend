using PadelManager.Application.DTOs.Court;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Application.Mappers.PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;


namespace PadelManager.Application.Services
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public CourtService(
            ICourtRepository courtRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _courtRepo = courtRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================

        public async Task<CourtResponseDto> AddNewCourtAsync(CreateCourtDto dto)
        {
            var court = dto.ToEntity();

            // AUDITORÍA
            var user = _currentUser.UserName ?? "System";
            court.CreatedBy = user;
            court.LastModifiedBy = user;

            await _courtRepo.AddAsync(court);
            await _unitOfWork.SaveChangesAsync();

            return court.ToResponseDto();
        }

        public async Task<bool> UpdateCourtAsync(Guid id, UpdateCourtDto dto)
        {
            var existingCourt = await _courtRepo.GetByIdAsync(id);
            if (existingCourt == null) return false;

            existingCourt.MapToEntity(dto);

            // AUDITORÍA
            existingCourt.LastModifiedBy = _currentUser.UserName ?? "System";
            existingCourt.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleCourtAsync(Guid id)
        {
            var court = await _courtRepo.GetByIdAsync(id);
            if (court == null) return false;

            // AUDITORÍA
            court.LastModifiedBy = _currentUser.UserName ?? "System";
            court.LastModifiedAt = DateTime.UtcNow;

            await _courtRepo.SoftDeleteToggleAsync(id);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================

        public async Task<CourtResponseDto?> GetCourtByIdAsync(Guid id)
        {
            var court = await _courtRepo.GetByIdAsync(id);
            return court?.ToResponseDto();
        }

        public async Task<IEnumerable<CourtResponseDto>> GetAllCourtsAsync()
        {
            var courts = await _courtRepo.GetAllAsync();
            return courts.ToResponseDto();
        }

        public async Task<IEnumerable<CourtResponseDto>> GetCourtsByVenueIdAsync(Guid venueId)
        {
            var courts = await _courtRepo.GetCourtsByVenueIdAsync(venueId);
            return courts.ToResponseDto();
        }

        public async Task<IEnumerable<CourtResponseDto>> GetCourtsByNameAsync(string name)
        {
            var courts = await _courtRepo.GetCourtsByNameAsync(name);
            return courts.ToResponseDto();
        }

        public async Task<IEnumerable<CourtResponseDto>> GetCourtsBySurfaceTypeAsync(string surfaceType)
        {
            var courts = await _courtRepo.GetCourtsBySurfaceTypeAsync(surfaceType);
            return courts.ToResponseDto();
        }

        public async Task<IEnumerable<CourtResponseDto>> GetCourtsByAvailabilityAsync(CourtAvailabilityType availability)
        {
            var courts = await _courtRepo.GetCourtsByAvailabilityAsync(availability);
            return courts.ToResponseDto();
        }
    }
}