using PadelManager.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IUserService
    {
        //CONSULTAS
        Task<UserResponseDto?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserResponseDto>> GetAll();
        Task<IEnumerable<UserResponseDto>> GetUsersByNameAsync(string name);
        Task<IEnumerable<UserResponseDto>> GetUsersByLastNameAsync(string lastName);
        Task<UserResponseDto?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserResponseDto>> GetUsersByRoleNameAsync(string roleName);

        //CRUD
        Task<UserResponseDto> AddNewUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserBasicInfoAsync(Guid id, UpdateUserDto dto);
        Task<bool> SoftDeleteUserAsync(Guid id);
    }
}
