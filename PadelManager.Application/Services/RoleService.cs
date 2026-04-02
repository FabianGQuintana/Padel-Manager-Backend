using PadelManager.Application.DTOs.Role;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepo;

        public RoleService(IRoleRepository roleRepo) => _roleRepo = roleRepo;

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepo.GetAllAsync();
            return roles.Select(r => r.ToDto());
        }
    }
}
