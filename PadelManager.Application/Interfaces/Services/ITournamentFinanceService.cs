using PadelManager.Application.DTOs.TournamentFinance;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Services
{
    public interface ITournamentFinanceService
    {
        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================
        Task<TournamentFinanceResponseDto> RegisterMovementAsync(CreateTournamentFinanceDto dto);
        Task<bool> UpdateMovementAsync(Guid id, UpdateTournamentFinanceDto dto);
        Task<bool> SoftDeleteToggleFinanceAsync(Guid id);

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================
        Task<TournamentFinanceResponseDto?> GetFinanceByIdAsync(Guid id);
        Task<IEnumerable<TournamentFinanceResponseDto>> GetAllFinancesAsync();
        Task<IEnumerable<TournamentFinanceResponseDto>> GetFinancesByTournamentIdAsync(Guid tournamentId);
        Task<IEnumerable<TournamentFinanceResponseDto>> GetFinancesByTypeAsync(TypeMovement type);

        // Útil para reportes mensuales o por fin de semana de torneo
        Task<decimal> GetTotalBalanceByTournamentAsync(Guid tournamentId);
    }
}