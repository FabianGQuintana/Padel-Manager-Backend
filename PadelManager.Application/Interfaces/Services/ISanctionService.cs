using PadelManager.Application.DTOs.Sanction;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ISanctionService
    {
        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================
        Task<SanctionResponseDto> AddNewSanctionAsync(CreateSanctionDto createSanction);
        Task<bool> UpdateSanctionAsync(Guid id, UpdateSanctionDto updateSanction);
        Task<bool> SoftDeleteToggleSanctionAsync(Guid id);

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================
        Task<SanctionResponseDto?> GetSanctionByIdAsync(Guid id);
        Task<IEnumerable<SanctionResponseDto>> GetAllSanctionsAsync();
        Task<IEnumerable<SanctionResponseDto>> GetSanctionsByPlayerIdAsync(Guid playerId);
        Task<IEnumerable<SanctionResponseDto>> GetSanctionsBySeverityAsync(StatusSeverity severity);

        // Devuelve solo las que están vigentes hoy (útil para el dashboard)
        Task<IEnumerable<SanctionResponseDto>> GetActiveSanctionsAsync();

        // Está habilitado para jugar?
        Task<bool> IsPlayerBlockedAsync(Guid playerId);
    }
}