using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ICoupleAvailabilityRepository : IGenericRepository<CoupleAvailability>
    {
        Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesByCoupleIdAsync(Guid coupleId);
        Task<IEnumerable<CoupleAvailability>> GetAvailabilitiesWithCouplesAsync();
        Task DeleteAvailabilitiesByCoupleIdAsync(Guid coupleId);
    }
}
