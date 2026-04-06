using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;

namespace PadelManager.Infrastructure.Repositories
{
    public class TournamentFinanceRepository : GenericRepository<TournamentFinance>, ITournamentFinanceRepository
    {
        public TournamentFinanceRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<TournamentFinance?> GetFinanceByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.TournamentFinances
                .FirstOrDefaultAsync(f => f.TournamentId == tournamentId);
        }
    }
}
