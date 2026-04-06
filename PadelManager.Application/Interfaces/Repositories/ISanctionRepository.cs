using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ISanctionRepository : IGenericRepository<Sanction>
    {
        Task<IEnumerable<Sanction>> GetSanctionsBySeverityAsync(string severity);
        Task<IEnumerable<Sanction>> GetSanctionsByPlayerIdAsync(Guid playerId);
        Task<IEnumerable<Sanction>> GetActiveSanctionsAsync();
    }
}
