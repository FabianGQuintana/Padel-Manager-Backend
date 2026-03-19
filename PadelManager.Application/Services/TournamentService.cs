using PadelManager.Application.DTOs.Tournament;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepo;
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IManagerRepository _managerRepo;

        public TournamentService(ITournamentRepository tournamentRepo, IRegistrationRepository registrationRepo, IManagerRepository managerRepo)
        {
            _tournamentRepo = tournamentRepo;
            _registrationRepo = registrationRepo;
            _managerRepo = managerRepo;
        }

        #region CRUD BÁSICO

        public async Task<TournamentResponseDto> AddNewTournamentAsync(CreateTournamentDto dto)
        {
            var tournament = dto.ToEntity();
            var result = await _tournamentRepo.AddAsync(tournament);
            // Al ser nuevo, las registraciones son 0 por defecto
            return result.ToResponseDto(0);
        }

        public async Task<bool> UpdateTournamentAsync(Guid id, UpdateTournamentDto dto)
        {
            var existingTournament = await _tournamentRepo.GetByIdAsync(id);
            if (existingTournament == null) return false;

            existingTournament.MapToEntity(dto);
            await _tournamentRepo.UpdateAsync(existingTournament);
            return true;
        }

        public async Task<bool> SoftDeleteToggleTournamentAsync(Guid id)
        {
            // El repo devuelve la entidad (Tournament), evaluamos si no es nula para devolver el bool
            var result = await _tournamentRepo.SoftDeleteToggleAsync(id);
            return result != null;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> CloseRegistrationsAndStartAsync(Guid tournamentId)
        {
            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);
            if (tournament == null) return false;

            var totalRegistrations = await _registrationRepo.CountByTournamentIdAsync(tournamentId);

            if (!tournament.CanStart(totalRegistrations))
            {
                throw new InvalidOperationException($"No se cumple el mínimo APA de 6 parejas. Actualmente hay: {totalRegistrations}");
            }

            tournament.Status = TournamentStatus.InProgress;
            await _tournamentRepo.UpdateAsync(tournament);
            return true;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<TournamentResponseDto?> GetTournamentByIdAsync(Guid tournamentId)
        {
            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);
            if (tournament == null) return null;

            // Buscamos las registraciones reales para que el DTO muestre si está lleno o no
            var count = await _registrationRepo.CountByTournamentIdAsync(tournamentId);
            return tournament.ToResponseDto(count);
        }

        public async Task<TournamentResponseDto?> GetTournamentByNameAsync(string name)
        {
            var tournament = await _tournamentRepo.GetTournamentsByNameAsync(name);
            return tournament?.ToResponseDto(0);
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetAllTournamentsAsync()
        {
            var tournaments = await _tournamentRepo.GetAllAsync();
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerIdAsync(Guid managerId)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByManagerIdAsync(managerId);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByStatusAsync(TournamentStatus status)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByStatusAsync(status);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByTypeAsync(string type)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByTypeAsync(type);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByStartDateAsync(DateTime startDate)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByStartDateAsync(startDate);
            return tournaments.ToResponseDto();
        }

        #endregion

        #region BÚSQUEDAS POR MANAGER

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerEmailAsync(string email)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByManagerEmailAsync(email);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerDniAsync(string dni)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByManagerDniAsync(dni);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentsByManagerNameAsync(string name)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByManagerNameAsync(name);
            return tournaments.ToResponseDto();
        }

        #endregion
    }
}