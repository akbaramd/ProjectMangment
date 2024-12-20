using PMS.Application.UseCases.Tenants.Models;

namespace PMS.Application.UseCases.Tenants;

public interface ITenantRoleService
{
    Task<List<TenantRoleDto>> GetRolesForTenantAsync(string tenantId);
    Task AddRoleAsync(string tenantId, TenantRoleCreateDto tenantRoleCreateDto);
    Task UpdateRoleAsync(string tenantId, Guid roleId, TenantRoleUpdateDto tenantRoleUpdateDto);
    Task DeleteRoleAsync(string tenantId, Guid roleId);
    Task<List<TenantPermissionGroupDto>> GetPermissionGroupsAsync();
}