using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ICourtRepository : IGenericRepository<Court>
    {
        Task<IEnumerable<Court>> GetCourtsByNameAsync(string name);
        Task<IEnumerable<Court>> GetCourtsBySurfaceTypeAsync(string superfaceType);
        Task<IEnumerable<Court>> GetCourtsByVenueIdAsync(Guid venueId);
        Task<IEnumerable<Court>> GetCourtsByAvailabilityAsync(CourtAvailabilityType availability);
    }
}
