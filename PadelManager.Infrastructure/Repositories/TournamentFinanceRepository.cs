using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class TournamentFinanceRepository : GenericRepository<TournamentFinance>, ITournamentFinanceRepository
    {
        public TournamentFinanceRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<IEnumerable<TournamentFinance>> GetFinancesByTournamentIdAsync(Guid tournamentId)
        {
            return await _context.TournamentFinances
                .Where(f => f.TournamentId == tournamentId && f.DeletedAt == null)
                .OrderByDescending(f => f.DateMovement) // Lo más nuevo arriba
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentFinance>> GetFinancesByTypeAsync(TypeMovement type)
        {
            return await _context.TournamentFinances
                .Where(f => f.MovementType == type && f.DeletedAt == null)
                .ToListAsync();
        }
    }
}
