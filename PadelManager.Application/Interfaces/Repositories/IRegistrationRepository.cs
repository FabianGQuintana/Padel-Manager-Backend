using System;
using PadelManager.Domain.Entities;


namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository : IGenericRepository<Registration>
    {
        Task<IEnumerable<Registration>> GetRegistrationsByDateAsync(DateTime date);
          
        Task<IEnumerable<Registration>> GetRegistrationsByTimeAsync(TimeOnly time);

        Task<Registration?> GetRegistrationByCoupleIdAsync(Guid coupleId);

        Task<IEnumerable<Registration>> GetRegistrationsByCategoryId(Guid categoryId);

        Task<IEnumerable<Registration>> GetRegistrationsByDetailsAsync(DateTime date, TimeOnly time, Guid coupleId, Guid categoryId);

        Task<int> CountRegistrationsByCategoryIdAsync(Guid categoryId);

        Task<int> CountByTournamentIdAsync(Guid tournamentId);
    }
}
