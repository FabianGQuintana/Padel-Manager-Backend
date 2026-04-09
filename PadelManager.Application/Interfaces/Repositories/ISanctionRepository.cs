using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ISanctionRepository : IGenericRepository<Sanction>
    {
        Task<IEnumerable<Sanction>> GetSanctionsBySeverityAsync(StatusSeverity severity);
        Task<IEnumerable<Sanction>> GetSanctionsByPlayerIdAsync(Guid playerId);
        Task<IEnumerable<Sanction>> GetActiveSanctionsAsync();
    }
}
