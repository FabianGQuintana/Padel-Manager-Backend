using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;     

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Task<IEnumerable<Tournament>> GetTournamentsByNameAsync(string name);
        
        Task<IEnumerable<Tournament>> GetTournamentsByStartDateAsync(DateTime startDate);

        Task<IEnumerable<Tournament>> GetTournamentsByStatusAsync(TournamentStatus status);

        Task<IEnumerable<Tournament>> GetTournamentsByTypeAsync(string tournamentType);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerIdAsync(Guid managerId);

        Task<IEnumerable<Tournament>> GetTournamentsByCategoryIdAsync(Guid categoryId);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerEmailAsync(string email);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerDniAsync(string dni);

        Task<IEnumerable<Tournament>> GetTournamentsByMaxTeamsAsync(int maxTeams);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerNameAsync(string name);

        Task<Tournament?> GetTournamentWithCategoriesAsync(Guid id);
    }
}
