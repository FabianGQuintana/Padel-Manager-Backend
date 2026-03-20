using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using System;

namespace PadelManager.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        
        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<ManagerResponseDto> AddNewManagerAsync(CreateManagerDto dto)
        {
            var manager = dto.ToEntity();

            var existingEmail = await _managerRepository.GetManagerByEmailAsync(dto.Email);

            if (existingEmail != null)
                throw new InvalidOperationException($"El email '{dto.Email}' ya se encuentra registrado.");

            var existingDni = await _managerRepository.GetManagerByDNIAsync(dto.Dni);

            if (existingDni != null)
                throw new InvalidOperationException($"El DNI '{dto.Dni}' ya pertenece a otro organizador.");

            var existingPhone = await _managerRepository.GetManagerByPhoneAsync(dto.PhoneNumber);

            if (existingPhone != null)
                throw new InvalidOperationException($"El número '{dto.PhoneNumber}' ya está en uso.");
            var newManager = await _managerRepository.AddAsync(manager);

            return newManager.ToResponseDto();
        }

        public async Task<bool> UpdateManagerAsync(Guid id, UpdateManagerDto dto)
        {
            var existingManager = await _managerRepository.GetByIdAsync(id);
            if (existingManager == null) return false;

            if (dto.Email != null && dto.Email != existingManager.Email)
            {
                var emailInUse = await _managerRepository.GetManagerByEmailAsync(dto.Email);
                if (emailInUse != null) throw new InvalidOperationException("El nuevo Email ya está en uso.");
            }

            if (dto.Dni != null && dto.Dni != existingManager.Dni)
            {
                var dniInUse = await _managerRepository.GetManagerByDNIAsync(dto.Dni);
                if (dniInUse != null) throw new InvalidOperationException("El nuevo DNI ya está en uso.");
            }

            if (dto.PhoneNumber != null && dto.PhoneNumber != existingManager.PhoneNumber)
            {
                var phoneNumberInUse = await _managerRepository.GetManagerByPhoneAsync(dto.PhoneNumber);
                if (phoneNumberInUse != null) throw new InvalidOperationException("El nuevo Número de telefono ya está en uso.");
            }

            existingManager.MapToEntity(dto);
            await _managerRepository.UpdateAsync(existingManager);

            return true;
        }

        public async Task<bool> SoftDeleteToggleManagerAsync(Guid id) {

            var result = await _managerRepository.SoftDeleteToggleAsync(id);
            return result != null;
        }


        public async Task<ManagerResponseDto?> GetManagerByIdAsync(Guid managerId)
        {
            var manager = await _managerRepository.GetByIdAsync(managerId);

            if (manager == null) return null;

            return manager.ToResponseDto();
        }

        public async Task<IEnumerable<ManagerResponseDto>> GetManagersByNameAsync(string name)
        {
            var managersName = await _managerRepository.GetManagersByNameAsync(name);
            return managersName.ToResponseDto();
        }

        public async Task<IEnumerable<ManagerResponseDto>> GetManagersByLastNameAsync(string lastName)
        {
            var managersLastName = await _managerRepository.GetManagersByLastNameAsync(lastName);
            return managersLastName.ToResponseDto();
        }

        public async Task<ManagerResponseDto?> GetManagerByDniAsync(string dni)
        {
            var managerId = await _managerRepository.GetManagerByDNIAsync(dni);
            return managerId?.ToResponseDto();
        }

        public async Task<ManagerResponseDto?> GetManagerByEmailAsync(string email)
        {
            var managerEmail = await _managerRepository.GetManagerByEmailAsync(email);
            return managerEmail?.ToResponseDto();
        }

        public async Task<ManagerResponseDto?> GetManagerByPhoneNumberAsync(string phoneNumber)
        {
            var managerPhoneNumber = await _managerRepository.GetManagerByPhoneAsync(phoneNumber);
            return managerPhoneNumber?.ToResponseDto();
        }

        public async Task<ManagerResponseDto?> GetManagerWithTournamentsAsync(Guid managerId)
        {
            var manager = await _managerRepository.GetManagerWithTournamentsAsync(managerId);

            // 2. Si no existe, devolvemos null (el controlador se encargará del 404)
            if (manager == null) return null;

            
            return manager.ToResponseDto();
        }
    }
}
