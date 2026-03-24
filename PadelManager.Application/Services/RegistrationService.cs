using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;

namespace PadelManager.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IUnitOfWork _unitOfWork;

        // Inyectamos el repositorio y el UnitOfWork igual que en TournamentService
        public RegistrationService(IRegistrationRepository registrationRepo, IUnitOfWork unitOfWork)
        {
            _registrationRepo = registrationRepo;
            _unitOfWork = unitOfWork;
        }

        #region CRUD BÁSICO

        public async Task<RegistrationResponseDto> AddNewRegistrationAsync(CreateRegistrationDto dto)
        {
            // 1. Lógica de negocio: Validamos que la pareja no esté ya inscripta en esa categoría
            bool alreadyExists = await IsCoupleAlreadyRegisteredAsync(dto.CoupleId, dto.CategoryId);
            if (alreadyExists)
            {
                throw new InvalidOperationException("La pareja ya se encuentra inscripta en esta categoría.");
            }

            // 2. Mapeamos (acá el Mapper le clava la hora y fecha exacta del servidor)
            var registration = dto.ToEntity();

            // 3. Guardamos a través del repositorio
            var result = await _registrationRepo.AddAsync(registration);

            // 4. Confirmamos la transacción con UnitOfWork
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateRegistrationAsync(Guid id, UpdateRegistrationDto dto)
        {
            var existingRegistration = await _registrationRepo.GetByIdAsync(id);
            if (existingRegistration == null) return false;

            existingRegistration.MapToEntity(dto);
            await _registrationRepo.UpdateAsync(existingRegistration);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleRegistrationAsync(Guid id)
        {
            var result = await _registrationRepo.SoftDeleteToggleAsync(id);
            if (result == null) return false;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> IsCoupleAlreadyRegisteredAsync(Guid coupleId, Guid categoryId)
        {
            // Traemos las inscripciones de esta pareja para verificar
            var coupleRegistrations = await _registrationRepo.GetRegistrationsByCoupleIdAsync(coupleId);

            // Si hay alguna inscripción activa (no borrada lógicamente) que coincida con la categoría, da true
            return coupleRegistrations.Any(r => r.CategoryId == categoryId && r.DeletedAt == null);
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<RegistrationResponseDto?> GetRegistrationByIdAsync(Guid registrationId)
        {
            var registration = await _registrationRepo.GetByIdAsync(registrationId);

            // Si el Repositorio usa .Include(r => r.Couple) etc, el Mapper lo detectará y llenará los nombres de forma automática.
            return registration?.ToResponseDto();
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync()
        {
            var registrations = await _registrationRepo.GetAllAsync();
            return registrations.ToResponseDto();
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByTournamentIdAsync(Guid tournamentId)
        {
            var registrations = await _registrationRepo.GetRegistrationsByTournamentIdAsync(tournamentId);
            return registrations.ToResponseDto();
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCategoryIdAsync(Guid categoryId)
        {
           
            var registrations = await _registrationRepo.GetRegistrationsByCategoryIdAsync(categoryId);
            return registrations.ToResponseDto();
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCoupleIdAsync(Guid coupleId)
        {
            var registrations = await _registrationRepo.GetRegistrationsByCoupleIdAsync(coupleId);
            return registrations.ToResponseDto();
        }


        public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByDateAsync(DateOnly date)
        {

            var dateTime = date.ToDateTime(TimeOnly.MinValue);

            var registrations = await _registrationRepo.GetRegistrationsByDateAsync(dateTime);

            return registrations.ToResponseDto();
        }

        #endregion
    }
}