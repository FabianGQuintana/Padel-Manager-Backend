using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {

        Task<IEnumerable<Match>> GetMatchesByWinnerAsync(Guid coupleId);
        Task<IEnumerable<Match>> GetMatchesByPlayerIdAsync(Guid playerId);
        Task<IEnumerable<Match>> GetMatchesByStageIdAsync(Guid stageId); 
        Task<IEnumerable<Match>> GetMatchesByZoneIdAsync(Guid zoneId);
        Task<IEnumerable<Match>> GetMatchesByCoupleIdAsync(Guid coupleId);
        Task<IEnumerable<Match>> GetMatchesByDateAsync(DateTime date);
        Task<IEnumerable<Match>> GetMatchesByLocationAsync(string location);
        Task<IEnumerable<Match>> GetMatchesByCourtAsync(string courtName);
        Task<IEnumerable<Match>> GetMatchesByStatusAsync (MatchStatus status);
        Task<Match?> GetMatchWithDetailsByIdAsync(Guid matchId);
        Task<int> CountByStageIdAsync(Guid stageId);
    }
}