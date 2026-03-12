using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;     

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        Task<Tournament?> GetTournamentByName(string name);
        
        Task<IEnumerable<Tournament>> GetTournamentByStartDate(DateTime startDate);

        Task<IEnumerable<Tournament>> GetTournamentByStatus(TournamentStatus status);

        Task<IEnumerable<Tournament>> GetTournamentByType(string tournamentType);

        Task<IEnumerable<Tournament>> GetTournamentByMaxTeamsPerCategory(int maxTeams);

        Task<IEnumerable<Tournament>> GetTournamentsByManagerId(Guid managerId);

        Task<IEnumerable<Tournament>> GetTournamentsByCategoryId(Guid categoryId);
    }
}
