using Bonyan.TenantManagement.Domain;
using PMS.Application.UseCases.Tenants.Exceptions;
using PMS.Application.UseCases.Tenants.Models;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Application.UseCases.Tenants
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

        // Fetch roles for a given tenantEntity
        public async Task<List<TenantRoleDto>> GetRolesForTenantAsync(string tenantId)
        {
            var tenant = await _tenantRepository.FindOneAsync(x=>x.Key == tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var roles = _roleRepository.GetByTenantId(tenant.Id);

            var roleDtos = roles.Select(r => new TenantRoleDto
            {
                Id = r.Id,
                Name = r.Key,
                Title = r.Title,
                Deletable = r.Deletable,
                IsSystemRole = r.IsSystemRole,
                Permissions = r.Permissions.Select(p => new TenantPermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return roleDtos;
        }

        // Add a new role to a tenantEntity with permissions
        public async Task AddRoleAsync(string tenantId, TenantRoleCreateDto tenantRoleCreateDto)
        {
            var tenant = await _tenantRepository.FindOneAsync(x=>x.Key == tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = new TenantRoleEntity(tenantRoleCreateDto.RoleName, tenant.Id,deletable: true, isSystemRole: false);
            await _roleRepository.AddAsync(role);

            foreach (var permissionKey in tenantRoleCreateDto.PermissionKeys)
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

        // Update role permissions and name for a tenantEntity
        public async Task UpdateRoleAsync(string tenantId, Guid roleId, TenantRoleUpdateDto tenantRoleUpdateDto)
        {
            var tenant = await _tenantRepository.FindOneAsync(x=>x.Key == tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id.Value)
            {
                throw new Exception();
            }

            // Update role name
            role.Key = tenantRoleUpdateDto.RoleName;

            // Clear existing permissions
            role.Permissions.Clear();

            // Add updated permissions
            foreach (var permissionKey in tenantRoleUpdateDto.PermissionKeys)
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
            var tenant = await _tenantRepository.FindOneAsync(x=>x.Key == tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id.Value)
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
        public async Task<List<TenantPermissionGroupDto>> GetPermissionGroupsAsync()
        {
            var permissionGroups = await _permissionRepository.GetPermissionGroupsAsync();

            var permissionGroupDtos = permissionGroups.Select(pg => new TenantPermissionGroupDto
            {
                Key = pg.Key,
                Name = pg.Name,
                Permissions = pg.Permissions.Select(p => new TenantPermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return permissionGroupDtos;
        }
    }
}
