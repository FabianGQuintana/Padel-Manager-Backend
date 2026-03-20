using PadelManager.Application.DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IManagerService
    {
        Task<ManagerResponseDto> AddNewManagerAsync(CreateManagerDto dto);

        Task<bool> UpdateManagerAsync(Guid id,UpdateManagerDto dto);

        Task<bool> SoftDeleteToggleManagerAsync(Guid id);

        Task<ManagerResponseDto?> GetManagerByIdAsync(Guid managerId);
        Task<IEnumerable<ManagerResponseDto>> GetManagersByNameAsync(string name);
        Task<IEnumerable<ManagerResponseDto>>GetManagersByLastNameAsync(string lastName);
        Task<ManagerResponseDto?> GetManagerByDniAsync(string dni);
        Task<ManagerResponseDto?> GetManagerByEmailAsync(string email);
        Task<ManagerResponseDto?> GetManagerByPhoneNumberAsync(string phoneNumber);

        Task<ManagerResponseDto?> GetManagerWithTournamentsAsync(Guid managerId);
    }
}
