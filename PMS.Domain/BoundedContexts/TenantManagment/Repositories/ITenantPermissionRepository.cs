namespace PMS.Domain.BoundedContexts.TenantManagment.Repositories;

public interface IPermissionRepository
{
    Task<TenantPermissionEntity?> GetPermissionByKeyAsync(string permissionKey);
    Task<List<TenantPermissionGroupEntity>> GetPermissionGroupsAsync();
    Task<TenantPermissionGroupEntity?> GetPermissionGroupByKeyAsync(string groupKey);
    Task<List<TenantPermissionEntity>> GetPermissionsByKeysAsync(IEnumerable<string> keys);
}