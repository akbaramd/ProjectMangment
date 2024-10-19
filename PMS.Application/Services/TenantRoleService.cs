using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Application.Exceptions;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Domain.Entities;

namespace PMS.Application.Services
{
    public class TenantRoleService : ITenantRoleService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public TenantRoleService(
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository)
        {
            _tenantRepository = tenantRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        // Fetch roles for a given tenant
        public async Task<List<RoleWithPermissionsDto>> GetRolesForTenantAsync(string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var roles = _roleRepository.GetByTenantId(tenant.Id);

            var roleDtos = roles.Select(r => new RoleWithPermissionsDto
            {
                Id = r.Id,
                Name = r.Key,
                Title = r.Title,
                Deletable = r.Deletable,
                IsSystemRole = r.IsSystemRole,
                Permissions = r.Permissions.Select(p => new PermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return roleDtos;
        }

        // Add a new role to a tenant with permissions
        public async Task AddRoleAsync(string tenantId, CreateRoleDto createRoleDto)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = new TenantRole(createRoleDto.RoleName, tenantId: tenant.Id, deletable: true, isSystemRole: false);
            await _roleRepository.AddAsync(role);

            foreach (var permissionKey in createRoleDto.PermissionKeys)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission == null)
                {
                    throw new Exception($"Permission '{permissionKey}' not found.");
                }

                role.Permissions.Add(permission);
            }

            await _roleRepository.UpdateAsync(role);
        }

        // Update role permissions and name for a tenant
        public async Task UpdateRoleAsync(string tenantId, Guid roleId, UpdateRoleDto updateRoleDto)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            // Update role name
            role.Key = updateRoleDto.RoleName;

            // Clear existing permissions
            role.Permissions.Clear();

            // Add updated permissions
            foreach (var permissionKey in updateRoleDto.PermissionKeys)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission == null)
                {
                    throw new Exception($"Permission '{permissionKey}' not found.");
                }

                role.Permissions.Add(permission);
            }

            await _roleRepository.UpdateAsync(role);
        }

        // Delete a role if no users are assigned
        public async Task DeleteRoleAsync(string tenantId, Guid roleId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            // Check if the role is deletable and delete it
            if (role.Deletable)
            {
                await _roleRepository.DeleteAsync(role);
            }
            else
            {
                throw new InvalidOperationException("Cannot delete system roles.");
            }
        }

        // Fetch permission groups and their permissions
        public async Task<List<PermissionGroupDto>> GetPermissionGroupsAsync()
        {
            var permissionGroups = await _permissionRepository.GetPermissionGroupsAsync();

            var permissionGroupDtos = permissionGroups.Select(pg => new PermissionGroupDto
            {
                Key = pg.Key,
                Name = pg.Name,
                Permissions = pg.Permissions.Select(p => new PermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return permissionGroupDtos;
        }
    }
}
