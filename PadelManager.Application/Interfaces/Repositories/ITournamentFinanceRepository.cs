using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ITournamentFinanceRepository : IGenericRepository<TournamentFinance>
    {
        Task<TournamentFinance?> GetFinanceByTournamentIdAsync(Guid tournamentId);

    }
}
