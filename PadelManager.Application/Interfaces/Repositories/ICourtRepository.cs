using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ICourtRepository : IGenericRepository<Court>
    {
        Task<IEnumerable<Court>> GetCourtsByNameAsync(string name);
        Task<IEnumerable<Court>> GetCourtsBySuperfaceTypeAsync(string superfaceType);
        Task<IEnumerable<Court>> GetCourtsByVenueIdAsync(Guid venueId);
        Task<IEnumerable<Court>> GetCourtsByAvailabilityAsync(string availability);
    }
}
