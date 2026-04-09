using PadelManager.Application.DTOs.Sanction;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Application.Mappers.PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;


namespace PadelManager.Application.Services
{
    public class SanctionService : ISanctionService
    {
        private readonly ISanctionRepository _sanctionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public SanctionService(
            ISanctionRepository sanctionRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _sanctionRepo = sanctionRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================

        public async Task<SanctionResponseDto> AddNewSanctionAsync(CreateSanctionDto dto)
        {
            // El ToEntity ya incluye la Severity (required) que arreglamos antes
            var sanction = dto.ToEntity();

            // AUDITORÍA
            var user = _currentUser.UserName ?? "System";
            sanction.CreatedBy = user;
            sanction.LastModifiedBy = user;

            await _sanctionRepo.AddAsync(sanction);
            await _unitOfWork.SaveChangesAsync();

            return sanction.ToResponseDto();
        }

        public async Task<bool> UpdateSanctionAsync(Guid id, UpdateSanctionDto dto)
        {
            var existingSanction = await _sanctionRepo.GetByIdAsync(id);
            if (existingSanction == null) return false;

            existingSanction.MapToEntity(dto);

            // AUDITORÍA
            existingSanction.LastModifiedBy = _currentUser.UserName ?? "System";
            existingSanction.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleSanctionAsync(Guid id)
        {
            var sanction = await _sanctionRepo.GetByIdAsync(id);
            if (sanction == null) return false;

            // AUDITORÍA
            sanction.LastModifiedBy = _currentUser.UserName ?? "System";
            sanction.LastModifiedAt = DateTime.UtcNow;

            await _sanctionRepo.SoftDeleteToggleAsync(id);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================

        public async Task<SanctionResponseDto?> GetSanctionByIdAsync(Guid id)
        {
            var sanction = await _sanctionRepo.GetByIdAsync(id);
            return sanction?.ToResponseDto();
        }

        public async Task<IEnumerable<SanctionResponseDto>> GetAllSanctionsAsync()
        {
            var sanctions = await _sanctionRepo.GetAllAsync();
            return sanctions.ToResponseDto();
        }

        public async Task<IEnumerable<SanctionResponseDto>> GetSanctionsByPlayerIdAsync(Guid playerId)
        {
            var sanctions = await _sanctionRepo.GetSanctionsByPlayerIdAsync(playerId);
            return sanctions.ToResponseDto();
        }

        public async Task<IEnumerable<SanctionResponseDto>> GetSanctionsBySeverityAsync(StatusSeverity severity)
        {
            var sanctions = await _sanctionRepo.GetSanctionsBySeverityAsync(severity);
            return sanctions.ToResponseDto();
        }

        public async Task<IEnumerable<SanctionResponseDto>> GetActiveSanctionsAsync()
        {
            // Filtramos las que no están borradas y cuya fecha de expiración aún no llegó
            var allSanctions = await _sanctionRepo.GetAllAsync();
            var activeOnes = allSanctions.Where(s =>
                s.DeletedAt == null &&
                (!s.ExpirationDate.HasValue || s.ExpirationDate > DateTime.UtcNow));

            return activeOnes.ToResponseDto();
        }

        // ==========================================
        // LÓGICA DE NEGOCIO
        // ==========================================

        public async Task<bool> IsPlayerBlockedAsync(Guid playerId)
        {
            var sanctions = await _sanctionRepo.GetSanctionsByPlayerIdAsync(playerId);

            // Un jugador está bloqueado si tiene CUALQUIER sanción que:
            // 1. No esté borrada lógicamente.
            // 2. Sea de nivel "Alto" (Blacklist permanente hasta que se borre).
            // 3. O tenga una fecha de expiración mayor a hoy.

            return sanctions.Any(s => s.DeletedAt == null &&
                (s.Severity == StatusSeverity.Alto ||
                (s.ExpirationDate.HasValue && s.ExpirationDate > DateTime.UtcNow)));
        }
    }
}