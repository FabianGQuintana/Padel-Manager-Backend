using System;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<IEnumerable<Match>> GetMatchesByDateAsync(DateTime date);

        Task<IEnumerable<Match>> GetMatchesByStatusAsync(MatchStatus status);

        Task<IEnumerable<Match>> GetMatchesByLocationAsync(string locationName);

        Task<IEnumerable<Match>> GetMatchesByCourtAsync(string courtName);

        Task<IEnumerable<Match>> GetMatchesByStageIdAsync(Guid stageId);

        Task<IEnumerable<Match>> GetMatchesByZoneIdAsync(Guid zoneId);

        Task<IEnumerable<Match>> GetMatchesByCoupleIdAsync(Guid coupleId);

        Task<IEnumerable<Match>> GetMatchesByWinnerAsync(Guid coupleId);

        Task<IEnumerable<Match>> GetMatchesByPlayerIdAsync(Guid playerId);

        Task<Match?> GetMatchWithDetailsByIdAsync(Guid matchId);
    }
}