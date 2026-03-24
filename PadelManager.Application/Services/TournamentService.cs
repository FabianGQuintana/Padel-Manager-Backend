using PadelManager.Application.DTOs.Tournament;
using PadelManager.Application.Interfaces.Persistence;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepo;

        public TournamentService(ITournamentRepository tournamentRepo, IRegistrationRepository registrationRepo, IManagerRepository managerRepo, IUnitOfWork unitOfWork, ICategoryRepository categoryRepo)
        {
            _tournamentRepo = tournamentRepo;
            _registrationRepo = registrationRepo;
            _managerRepo = managerRepo;
            _unitOfWork = unitOfWork;
            _categoryRepo = categoryRepo;
        }

        #region CRUD BÁSICO

        public async Task<TournamentResponseDto> AddNewTournamentAsync(CreateTournamentDto dto)
        {
            var tournament = dto.ToEntity();
            var result = await _tournamentRepo.AddAsync(tournament);
            // Al ser nuevo, las registraciones son 0 por defecto
            await _unitOfWork.SaveChangesAsync();
            return result.ToResponseDto(0);
        }

        public async Task<bool> UpdateTournamentAsync(Guid id, UpdateTournamentDto dto)
        {
            var existingTournament = await _tournamentRepo.GetByIdAsync(id);
            if (existingTournament == null) return false;

            existingTournament.MapToEntity(dto);
            await _tournamentRepo.UpdateAsync(existingTournament);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleTournamentAsync(Guid id)
        {
            var result = await _tournamentRepo.SoftDeleteToggleAsync(id);
            if (result == null) return false;

            
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> CloseRegistrationsAndStartAsync(Guid tournamentId)
        {
            // 1. Buscamos el torneo
            var tournament = await _tournamentRepo.GetByIdAsync(tournamentId);
            if (tournament == null) return false;

            // 2. Buscamos todas las categorías de este torneo (con sus inscripciones cargadas)
            // Usamos el método que creamos recién en el CategoryRepository
            var categories = await _categoryRepo.GetCategoriesByTournamentWithRegistrationsAsync(tournamentId);

            if (!categories.Any())
            {
                throw new InvalidOperationException("No se puede iniciar un torneo sin categorías.");
            }

            // 3. Validamos cada categoría usando la lógica que movimos a la entidad pura
            foreach (var category in categories)
            {
                int totalRegistrations = category.Registrations.Count;

                // Usamos el método 'CanStart' que ya tenés en la entidad Category
                if (!category.CanStart(totalRegistrations))
                {
                    throw new InvalidOperationException(
                        $"La categoría '{category.Name}' no cumple el mínimo APA de 6 parejas. " +
                        $"Actual tiene: {totalRegistrations}. Por favor, revisá los cupos antes de iniciar.");
                }
            }

            // 4. Si todas pasaron la validación, cambiamos el estado del torneo
            // Asegurate de si tu propiedad se llama Status o TournamentStatus (según tu Enum)
            tournament.Status = TournamentStatus.InProgress;

            // 5. Guardamos los cambios mediante la unidad de trabajo
            return await _unitOfWork.SaveChangesAsync() > 0;
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

        public async Task<IEnumerable<TournamentResponseDto>> GetTournamentByNameAsync(string name)
        {
            var tournaments = await _tournamentRepo.GetTournamentsByNameAsync(name);
            return tournaments.ToResponseDto();
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