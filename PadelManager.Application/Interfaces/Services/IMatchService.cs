using System;
using System.Collections.Generic;
using System.Text;
using PadelManager.Application.DTOs.Match;
using PadelManager.Domain.Enum;
using System.Threading.Tasks;


namespace PadelManager.Application.Interfaces.Services
{
    public interface IMatchService
    {
        
        // CRUD BÁSICO

        Task<MatchResponseDto> AddNewMatchAsync(CreateMatchDto dto);
        Task<bool> UpdateMatchAsync(Guid id, UpdateMatchDto dto);
        Task<bool> SoftDeleteToggleMatchAsync(Guid id);


        // LÓGICA DE NEGOCIO
        // Método específico para cuando termina el partido. 
        // Valida el ganador, actualiza los sets, cambia el estado a Finalizado 
        // y (en un futuro) podría disparar el evento para las estadísticas.
        Task<bool> LoadMatchResultAsync(Guid matchId, LoadMatchResultDto dto);


        // LECTURA
     
        Task<MatchResponseDto?> GetMatchByIdAsync(Guid matchId);
        Task<IEnumerable<MatchResponseDto>> GetAllMatchesAsync();
        Task<IEnumerable<MatchResponseDto>> GetMatchesByDateAsync(DateTime date);
        Task<IEnumerable<MatchResponseDto>> GetMatchesByStatusAsync(MatchStatus status);



        // FILTROS ESPECÍFICOS DEL TORNEO (Relaciones)

        Task<IEnumerable<MatchResponseDto>> GetMatchesByStageIdAsync(Guid stageId);
        Task<IEnumerable<MatchResponseDto>> GetMatchesByZoneIdAsync(Guid zoneId);
        Task<IEnumerable<MatchResponseDto>> GetMatchesByCoupleIdAsync(Guid coupleId);


        // Filtros logísticos para el organizador
        Task<IEnumerable<MatchResponseDto>> GetMatchesByLocationAsync(string locationName);
        Task<IEnumerable<MatchResponseDto>> GetMatchesByCourtAsync(string courtName);
    }
}