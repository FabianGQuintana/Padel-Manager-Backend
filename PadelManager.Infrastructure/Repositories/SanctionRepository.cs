using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PadelManager.Domain.Enum;
using PadelManager.Application.Interfaces.Repositories;
namespace PadelManager.Infrastructure.Repositories
{
    public class SanctionRepository : GenericRepository<Sanction>, ISanctionRepository
    {
        public SanctionRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<IEnumerable<Sanction>> GetSanctionsByPlayerIdAsync(Guid playerId)
        {
            return await _context.Sanctions
                .Where(s => s.PlayerId == playerId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sanction>> GetActiveSanctionsAsync()
        {
            // Sanciones que aún no vencieron
            return await _context.Sanctions
                .Where(s => s.ExpirationDate > DateTime.UtcNow && s.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sanction>> GetSanctionsBySeverityAsync(string severity)
        {
            if(!Enum.TryParse<StatusSeverity>(severity,true,out var statusEnum)){
                return Enumerable.Empty<Sanction>();
            }
            
            return await _context.Sanctions
               .Where(s => s.Severity == statusEnum)
               .ToListAsync();
        }
    }
}
