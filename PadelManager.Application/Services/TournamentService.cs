using PadelManager.Application.DTOs.Payment;
using PadelManager.Application.DTOs.Tournament;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
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
            var tournament = await _tournamentRepo.GetTournamentWithCategoriesAsync(id);

            if (tournament == null) return false;

          
            if (!tournament.IsDeleted) 
            {
                if (tournament.Categories != null && tournament.Categories.Any())
                {
                    throw new InvalidOperationException("No se puede eliminar el torneo porque tiene categorías asignadas.");
                }
            }

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

            // 1. Traemos las categorías y filtramos las que NO están borradas
            var allCategories = await _categoryRepo.GetCategoriesByTournamentWithRegistrationsAsync(tournamentId);

            // 🎯 FILTRO 1: Solo categorías activas (donde DeletedAt es null)
            var activeCategories = allCategories.Where(c => c.DeletedAt == null).ToList();

            if (!activeCategories.Any())
            {
                throw new InvalidOperationException("No se puede iniciar un torneo sin categorías activas.");
            }

            foreach (var category in activeCategories)
            {
                // FILTRO 2: Contamos solo las inscripciones activas de esa categoría
                int totalActiveRegistrations = category.Registrations.Count(r => r.DeletedAt == null);

                if (!category.CanStart(totalActiveRegistrations))
                {
                    throw new InvalidOperationException(
                        $"La categoría '{category.Name}' no cumple el mínimo APA de 6 parejas. " +
                        $"Actualmente tiene: {totalActiveRegistrations}. Por favor, revisá los cupos o eliminá la categoría.");
                }
            }

            // Si pasó todas las validaciones de las categorías activas:
            tournament.StatusType = TournamentStatus.InProgress;
            tournament.LastModifiedBy = _currentUser.UserName ?? "System";
            tournament.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> OpenTournamentRegistrationsAsync(Guid id)
        {
            
            var tournament = await _tournamentRepo.GetTournamentWithCategoriesAsync(id);
            if (tournament == null) return false;

            
            if (tournament.IsDeleted || tournament.Status != "Active")
            {
                throw new InvalidOperationException("No se pueden abrir inscripciones en un torneo eliminado o desactivado.");
            }

            
            if (tournament.StatusType != TournamentStatus.Draft)
            {
                throw new InvalidOperationException($"El torneo no está en modo Borrador. Estado actual: {tournament.StatusType}");
            }

            
            if (tournament.Categories == null || !tournament.Categories.Any())
            {
                throw new InvalidOperationException("No podés abrir las inscripciones de un torneo que no tiene categorías configuradas.");
            }

            //  Cambio de estado
            tournament.StatusType = TournamentStatus.RegistrationOpen;

            // Auditoría
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

        #region CONTABILIDAD Y FINANZAS 

        public async Task<TournamentFinancialSummaryDto?> GetFinancialSummaryAsync(Guid tournamentId)
        {
            
            var tournament = await _tournamentRepo.GetTournamentAccountingAsync(tournamentId);

            if (tournament == null) return null;

            // El Mapper hace toda la matemática que definimos (Señas, Descuentos, Deuda)
            return tournament.ToFinancialSummaryDto();
        }

        public async Task<List<CategoryPaymentGridDto>> GetCategoryPaymentGridsAsync(Guid tournamentId)
        {
            var tournament = await _tournamentRepo.GetTournamentAccountingAsync(tournamentId);

            if (tournament == null) return new List<CategoryPaymentGridDto>();

            // Genera las grillas agrupadas por categoría tal cual el PDF
            return tournament.ToCategoryGrids();
        }

        #endregion
    }
}