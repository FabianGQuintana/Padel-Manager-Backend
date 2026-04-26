using PadelManager.Application.DTOs.Payment;
using PadelManager.Application.DTOs.Tournament;
using PadelManager.Domain.Enum;

public interface ITournamentService
{
    // CRUD BÁSICO
    Task<TournamentResponseDto> AddNewTournamentAsync(CreateTournamentDto dto);
    Task<bool> UpdateTournamentAsync(Guid id, UpdateTournamentDto dto);
    Task<bool> SoftDeleteToggleTournamentAsync(Guid id);

    // LÓGICA DE NEGOCIO
    Task<bool> CloseRegistrationsAndStartAsync(Guid tournamentId);
    Task<bool> OpenTournamentRegistrationsAsync(Guid id);

    // LECTURA
    Task<TournamentResponseDto?> GetTournamentByIdAsync(Guid tournamentId);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentByNameAsync(string name);
    Task<IEnumerable<TournamentResponseDto>> GetAllTournamentsAsync();
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerIdAsync(Guid managerId);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByStatusAsync(TournamentStatus status);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByTypeAsync(string type);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByStartDateAsync(DateTime startDate);

    // Filtros por Manager (DNI/Email y Nombre)
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerEmailAsync(string email);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerDniAsync(string dni);
    Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerNameAsync(string name);

    //Contabilidad y finanzas.
    Task<TournamentFinancialSummaryDto?> GetFinancialSummaryAsync(Guid tournamentId);
    Task<List<CategoryPaymentGridDto>> GetCategoryPaymentGridsAsync(Guid tournamentId);
}