using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
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
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        
        public ManagerService(IManagerRepository managerRepository,ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _managerRepository = managerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        #region MÉTODOS DE ESCRITURA (POST - PUT - PATCH)

        public async Task<ManagerResponseDto> AddNewManagerAsync(CreateManagerDto dto)
        {
            // 1. Validaciones de Negocio (Duplicados)
            if (await _managerRepository.GetManagerByEmailAsync(dto.Email) != null)
                throw new InvalidOperationException($"El email '{dto.Email}' ya se encuentra registrado.");

            if (await _managerRepository.GetManagerByDNIAsync(dto.Dni) != null)
                throw new InvalidOperationException($"El DNI '{dto.Dni}' ya pertenece a otro organizador.");

            if (await _managerRepository.GetManagerByPhoneAsync(dto.PhoneNumber) != null)
                throw new InvalidOperationException($"El número '{dto.PhoneNumber}' ya está en uso.");

            // 2. Mapeo a Entidad
            var manager = dto.ToEntity();

            // 3. AUDITORÍA: ¿Quién está creando esto?
            // Usamos "System" por si es un autoregistro donde aún no hay token
            var userWhoCreated = _currentUser.UserName ?? "System";
            manager.CreatedBy = userWhoCreated;
            manager.LastModifiedBy = userWhoCreated;

            // 4. Persistencia mediante Repositorio y Unit of Work
            var newManager = await _managerRepository.AddAsync(manager);

            // IMPORTANTE: Sin esta línea, los cambios no llegan a SQL
            await _unitOfWork.SaveChangesAsync();

            return newManager.ToResponseDto();
        }

        public async Task<bool> UpdateManagerAsync(Guid id, UpdateManagerDto dto)
        {
            // 1. Buscar la entidad existente
            var existingManager = await _managerRepository.GetByIdAsync(id);
            if (existingManager == null) return false;

            // 2. Validaciones de integridad (si cambia Email, DNI o Teléfono)
            if (dto.Email != null && dto.Email != existingManager.Email)
            {
                if (await _managerRepository.GetManagerByEmailAsync(dto.Email) != null)
                    throw new InvalidOperationException("El nuevo Email ya está en uso.");
            }

            if (dto.Dni != null && dto.Dni != existingManager.Dni)
            {
                if (await _managerRepository.GetManagerByDNIAsync(dto.Dni) != null)
                    throw new InvalidOperationException("El nuevo DNI ya está en uso.");
            }

            if (dto.PhoneNumber != null && dto.PhoneNumber != existingManager.PhoneNumber)
            {
                if (await _managerRepository.GetManagerByPhoneAsync(dto.PhoneNumber) != null)
                    throw new InvalidOperationException("El nuevo número ya está en uso.");
            }

            // 3. Mapeo de cambios
            existingManager.MapToEntity(dto);

            // 4. AUDITORÍA: ¿Quién está modificando?
            existingManager.LastModifiedBy = _currentUser.UserName ?? "System";
            existingManager.LastModifiedAt = DateTime.UtcNow;

            // 5. Actualizar y Guardar
            await _managerRepository.UpdateAsync(existingManager);

            // Devolvemos true si se impactó al menos una fila en la DB
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleManagerAsync(Guid id)
        {
            // 1. Buscamos el manager para poder auditar quién lo borra
            var manager = await _managerRepository.GetByIdAsync(id);
            if (manager == null) return false;

            // 2. AUDITORÍA: Dejamos registro de quién hizo el cambio de estado
            manager.LastModifiedBy = _currentUser.UserName ?? "System";
            manager.LastModifiedAt = DateTime.UtcNow;

            // 3. Ejecutamos el toggle en el repo (cambia el DeletedAt o Status)
            await _managerRepository.SoftDeleteToggleAsync(id);

            // 4. Confirmamos el cambio
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion

        #region GETS
        public async Task<ManagerResponseDto?> GetManagerByIdAsync(Guid managerId)
        {
            var manager = await _managerRepository.GetByIdAsync(managerId);

            if (manager == null) return null;

            return manager.ToResponseDto();
        }

        public async Task<IEnumerable<ManagerResponseDto>> GetAllManagersAsync()
        {
            var managers = await _managerRepository.GetAllAsync();
            return managers.ToResponseDto();
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
        #endregion
}
