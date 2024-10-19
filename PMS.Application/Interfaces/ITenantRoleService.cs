using PMS.Application.DTOs;

namespace PMS.Application.Interfaces;

public interface ITenantRoleService
{
    Task<List<RoleWithPermissionsDto>> GetRolesForTenantAsync(string tenantId);
    Task AddRoleAsync(string tenantId, CreateRoleDto createRoleDto);
    Task UpdateRoleAsync(string tenantId, Guid roleId, UpdateRoleDto updateRoleDto);
    Task DeleteRoleAsync(string tenantId, Guid roleId);
    Task<List<PermissionGroupDto>> GetPermissionGroupsAsync();
}