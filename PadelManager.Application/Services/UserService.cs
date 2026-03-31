using PadelManager.Application.DTOs.User;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUser _currentUser;

        public UserService(
            IUserRepository userRepo,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ICurrentUser currentUser)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
        }

        #region CONSULTAS (READ)

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user?.ToResponseDto();
        }

        public async Task<IEnumerable<UserResponseDto>> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => u.ToResponseDto());
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersByNameAsync(string name)
        {
            // Nota: Asegurate de tener este método en el IUserRepository
            var users = await _userRepo.GetUsersByNameAsync(name);
            return users.Select(u => u.ToResponseDto());
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersByLastNameAsync(string lastName)
        {
            var users = await _userRepo.GetUsersByLastNameAsync(lastName);
            return users.Select(u => u.ToResponseDto());
        }

        public async Task<UserResponseDto?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userRepo.GetUserByPhoneNumberAsync(phoneNumber);
            return user?.ToResponseDto();
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            return user?.ToResponseDto();
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersByRoleNameAsync(string roleName)
        {
            var users = await _userRepo.GetUsersByRoleNameAsync(roleName);
            return users.Select(u => u.ToResponseDto());
        }

        #endregion

        #region CRUD (WRITE)

        public async Task<UserResponseDto> AddNewUserAsync(CreateUserDto dto)
        {
            // 1. Validar unicidad (Email y DNI)
            if (await _userRepo.GetUserByEmailAsync(dto.Email) != null)
                throw new InvalidOperationException("El email ya está registrado.");

            if (await _userRepo.GetUserByDniAsync(dto.Dni) != null)
                throw new InvalidOperationException("El DNI ya está registrado.");

            // 2. Mapear y Hashear
            var user = dto.ToEntity();
            user.PasswordHash = _passwordHasher.Hash(dto.Password);

            // 3. Auditoría básica
            var auditUser = _currentUser.UserName ?? "System";
            user.CreatedBy = auditUser;
            user.LastModifiedBy = auditUser;

            // 4. Persistir
            var result = await _userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateUserBasicInfoAsync(Guid id, UpdateUserDto dto)
        {
            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser == null) return false;

            // Actualizamos solo los campos permitidos
            existingUser.Name = dto.Name;
            existingUser.LastName = dto.LastName;
            existingUser.PhoneNumber = dto.PhoneNumber;

            // Auditoría
            existingUser.LastModifiedBy = _currentUser.UserName ?? "System";
            existingUser.LastModifiedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(existingUser);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteUserAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return false;

            // Auditoría del borrado
            user.LastModifiedBy = _currentUser.UserName ?? "System";
            user.LastModifiedAt = DateTime.UtcNow;

            // Usamos el Toggle que tenés en el Repositorio genérico
            await _userRepo.SoftDeleteToggleAsync(id);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        #endregion
    }
}