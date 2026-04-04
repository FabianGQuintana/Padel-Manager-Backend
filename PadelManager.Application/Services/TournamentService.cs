using PadelManager.Application.DTOs.Tournament;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepo;
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IManagerRepository _managerRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICurrentUser _currentUser;

        public TournamentService(
            ITournamentRepository tournamentRepo,
            IRegistrationRepository registrationRepo,
            IManagerRepository managerRepo,
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepo,
            ICurrentUser currentUser)
        {
            _tournamentRepo = tournamentRepo;
            _registrationRepo = registrationRepo;
            _managerRepo = managerRepo;
            _unitOfWork = unitOfWork;
            _categoryRepo = categoryRepo;
            _currentUser = currentUser;
        }

        #region CRUD BÁSICO

        public async Task<TournamentResponseDto> AddNewTournamentAsync(CreateTournamentDto dto)
        {

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("La fecha de inicio del torneo no puede ser anterior a hoy.");
            }


            var tournament = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            //PASO CLAVE: Buscamos al manager y lo agregamos a la colección
            var creatorManager = await _managerRepo.GetByIdAsync(dto.ManagerId);
            if (creatorManager != null)
            {
                tournament.Managers.Add(creatorManager); // Así se llena la tabla intermedia
            }

            tournament.CreatedBy = user;
            tournament.LastModifiedBy = user;

            await _tournamentRepo.AddAsync(tournament);
            await _unitOfWork.SaveChangesAsync();

            // Re-hidratamos con los Includes
            var enrichedTournament = await _tournamentRepo.GetTournamentByIdWithManagersAsync(tournament.Id);
            return enrichedTournament!.ToResponseDto(0);
        }

        public async Task<bool> UpdateTournamentAsync(Guid id, UpdateTournamentDto dto)
        {
            var existingTournament = await _tournamentRepo.GetByIdAsync(id);
            if (existingTournament == null) return false;

            existingTournament.MapToEntity(dto);

            existingTournament.LastModifiedBy = _currentUser.UserName ?? "System";
            existingTournament.LastModifiedAt = DateTime.UtcNow;

            await _tournamentRepo.UpdateAsync(existingTournament);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleTournamentAsync(Guid id)
        {
            var tournament = await _tournamentRepo.GetByIdAsync(id);
            if (tournament == null) return false;

            tournament.LastModifiedBy = _currentUser.UserName ?? "System";
            tournament.LastModifiedAt = DateTime.UtcNow;

            await _tournamentRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> CloseRegistrationsAndStartAsync(Guid tournamentId)
        {
            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);
            if (tournament == null) return false;

            var categories = await _categoryRepo.GetCategoriesByTournamentWithRegistrationsAsync(tournamentId);

            if (!categories.Any())
            {
                throw new InvalidOperationException("No se puede iniciar un torneo sin categorías.");
            }

            foreach (var category in categories)
            {
                int totalRegistrations = category.Registrations.Count;

                if (!category.CanStart(totalRegistrations))
                {
                    throw new InvalidOperationException(
                        $"La categoría '{category.Name}' no cumple el mínimo APA de 6 parejas. " +
                        $"Actual tiene: {totalRegistrations}. Por favor, revisá los cupos antes de iniciar.");
                }
            }

            tournament.StatusType = TournamentStatus.InProgress;
            tournament.LastModifiedBy = _currentUser.UserName ?? "System";
            tournament.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<TournamentResponseDto?> GetTournamentByIdAsync(Guid tournamentId)
        {
            var tournament = await _tournamentRepo.GetTournamentByIdWithManagersAsync(tournamentId);

            if (tournament == null) return null;

            var count = await _registrationRepo.CountByTournamentIdAsync(tournamentId);
            return tournament.ToResponseDto(count);
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentByNameAsync(string name)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByNameAsync(name);
            return tournaments.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetAllTournamentsAsync()
        {
            var tournaments = await _tournamentRepo.GetAllWithManagersAsync();
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