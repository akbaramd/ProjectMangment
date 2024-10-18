using PMS.Domain.Entities;

public interface IPermissionRepository
{
    Task<ApplicationPermission?> GetPermissionByKeyAsync(string permissionKey);
    Task<List<ApplicationPermissionGroup>> GetPermissionGroupsAsync();
    Task<ApplicationPermissionGroup?> GetPermissionGroupByKeyAsync(string groupKey);
    Task<List<ApplicationPermission>> GetPermissionsByKeysAsync(IEnumerable<string> keys);
}