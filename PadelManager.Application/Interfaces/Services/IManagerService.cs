using PadelManager.Application.DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IManagerService
    {
        Task<ManagerResponseDto?> GetManagerProfileAsync(Guid id);
        Task<IEnumerable<ManagerResponseDto>> GetAllManagersAsync();
        Task<bool> UpdateManagerProfileAsync(Guid id, UpdateManagerDto dto);
    }
}
