using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository : IGenericRepository<Registration>
    {
        Task<List<Couple>> GetCouplesByCategoryAsync(Guid categoryId);

        Task<IEnumerable<Registration>> GetRegistrationsByDateAsync(DateTime date);

        Task<IEnumerable<Registration>> GetRegistrationsByTimeAsync(TimeOnly time);

        Task<IEnumerable<Registration>> GetRegistrationsByCoupleIdAsync(Guid coupleId);

        Task<IEnumerable<Registration>> GetRegistrationsByCategoryIdAsync(Guid categoryId);

        Task<IEnumerable<Registration>> GetRegistrationsByDetailsAsync(DateTime date, TimeOnly time, Guid coupleId, Guid categoryId);

        Task<int> CountRegistrationsByCategoryIdAsync(Guid categoryId);

        Task<int> CountByTournamentIdAsync(Guid tournamentId);

        Task<bool> ExistsByCoupleAndTournamentAsync(Guid coupleId, Guid tournamentId);

        Task<IEnumerable<Registration>> GetRegistrationsByTournamentIdAsync(Guid tournamentId);
        Task<Registration?> GetRegistrationByIdWithDetailsAsync(Guid id);
        Task<bool> IsAnyPlayerAlreadyRegisteredInTournamentAsync(Guid tournamentId, Guid player1Id, Guid player2Id);
    }
}