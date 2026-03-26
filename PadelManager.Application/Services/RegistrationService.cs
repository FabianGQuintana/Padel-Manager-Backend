using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Mappers;

namespace PadelManager.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public RegistrationService(
            IRegistrationRepository registrationRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _registrationRepo = registrationRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        #region CRUD BÁSICO

        public async Task<RegistrationResponseDto> AddNewRegistrationAsync(CreateRegistrationDto dto)
        {
            bool alreadyExists = await IsCoupleAlreadyRegisteredAsync(dto.CoupleId, dto.CategoryId);
            if (alreadyExists)
            {
                throw new InvalidOperationException("La pareja ya se encuentra inscripta en esta categoría.");
            }

            var registration = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            registration.CreatedBy = user;
            registration.LastModifiedBy = user;

            var result = await _registrationRepo.AddAsync(registration);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateRegistrationAsync(Guid id, UpdateRegistrationDto dto)
        {
            var existingRegistration = await _registrationRepo.GetByIdAsync(id);
            if (existingRegistration == null) return false;

            existingRegistration.MapToEntity(dto);

            existingRegistration.LastModifiedBy = _currentUser.UserName ?? "System";
            existingRegistration.LastModifiedAt = DateTime.UtcNow;

            await _registrationRepo.UpdateAsync(existingRegistration);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleRegistrationAsync(Guid id)
        {
            var existingRegistration = await _registrationRepo.GetByIdAsync(id);
            if (existingRegistration == null) return false;

            existingRegistration.LastModifiedBy = _currentUser.UserName ?? "System";
            existingRegistration.LastModifiedAt = DateTime.UtcNow;

            await _registrationRepo.SoftDeleteToggleAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        public async Task<bool> IsCoupleAlreadyRegisteredAsync(Guid coupleId, Guid categoryId)
        {
            var coupleRegistrations = await _registrationRepo.GetRegistrationsByCoupleIdAsync(coupleId);
            return coupleRegistrations.Any(r => r.CategoryId == categoryId && r.DeletedAt == null);
        }

        #endregion

        #region LECTURA Y FILTROS

        public async Task<RegistrationResponseDto?> GetRegistrationByIdAsync(Guid registrationId)
        {
            var registration = await _registrationRepo.GetByIdAsync(registrationId);
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