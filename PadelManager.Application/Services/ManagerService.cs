using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ManagerService(IManagerRepository managerRepo, IUnitOfWork unitOfWork)
        {
            _managerRepo = managerRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ManagerResponseDto?> GetManagerProfileAsync(Guid id)
        {
            // Usamos el método específico del repo que hace el .Include(u => u.User)
            var manager = await _managerRepo.GetManagerWithUserAsync(id);

            if (manager == null) return null;

            // Mapeamos a ResponseDto (que ya tiene nombre, apellido, email, etc.)
            return manager.ToResponseDto();
        }

        public async Task<IEnumerable<ManagerResponseDto>> GetAllManagersAsync()
        {
            // Nota: Aquí podrías necesitar un método en el repo que traiga a todos con su User
            // Por ahora usamos el genérico y mapeamos
            var managers = await _managerRepo.GetAllAsync();

            return managers.Select(m => m.ToResponseDto());
        }

        public async Task<bool> UpdateManagerProfileAsync(Guid id, UpdateManagerDto dto)
        {
            // 1. Buscamos el Manager incluyendo su User para poder actualizar ambos
            var manager = await _managerRepo.GetManagerWithUserAsync(id);

            if (manager == null) return false;

            // 2. Usamos el Mapper para actualizar la entidad con los datos del DTO
            // Este mapper que definimos antes pisa tanto campos de Manager como de User
            manager.UpdateEntity(dto);

            // 3. Persistimos los cambios
            await _managerRepo.UpdateAsync(manager);

            // 4. Confirmamos en la DB vía Unit of Work
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}