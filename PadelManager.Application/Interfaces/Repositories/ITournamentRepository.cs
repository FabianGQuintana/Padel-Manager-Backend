using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;     

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Task<Tournament?> GetTournamentsByNameAsync(string name);
        
        Task<IEnumerable<Tournament>> GetTournamentsByStartDateAsync(DateTime startDate);

        Task<IEnumerable<Tournament>> GetTournamentsByStatusAsync(TournamentStatus status);

        Task<IEnumerable<Tournament>> GetTournamentsByTypeAsync(string tournamentType);

        Task<IEnumerable<Tournament>> GetTournamentsByMaxTeamsPerCategoryAsync(int maxTeams);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerIdAsync(Guid managerId);

        Task<IEnumerable<Tournament>> GetTournamentsByCategoryIdAsync(Guid categoryId);
    }
}
