using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IVenueRepository : IGenericRepository<Venue>
    {
        Task<Venue?> GetVenueByNameAsync(string name);
        Task<Venue?> GetVenueByAddressAsync(string address);
        Task<Venue?> GetVenueByPhoneNumberAsync(string phoneNumber);
        Task<Venue?> GetVenueWithCourtsAsync(Guid id);
        Task<IEnumerable<Venue>> GetVenuesByCityAsync(string city);
    }
}
