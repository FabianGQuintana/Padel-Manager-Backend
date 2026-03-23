using System;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        public Task<IEnumerable<Player>> GetPlayerByNameAsync(string name);

        public Task<IEnumerable<Player>> GetPlayerByLastNameAsync(string lastName);

        public Task<Player?> GetPlayerByPhoneNumberAsync(string phoneNumber);
        
        public Task<Player?> GetPlayerByDniAsync(string dni);

        public Task<IEnumerable<Player>> GetPlayerByAgeAsync(Byte age);

    }
}
