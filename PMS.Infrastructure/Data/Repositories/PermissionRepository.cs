using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;

namespace PMS.Infrastructure.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve a permission by its key
        public async Task<TenantPermissionEntity?> GetPermissionByKeyAsync(string permissionKey)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Key == permissionKey);
        }

        // Retrieve all permission groups along with their permissions
        public async Task<List<TenantPermissionGroupEntity>> GetPermissionGroupsAsync()
        {
            return await _context.PermissionGroups
                .Include(pg => pg.Permissions)  // Include the related permissions
                .ToListAsync();
        }

        // Retrieve a permission group by its key
        public async Task<TenantPermissionGroupEntity?> GetPermissionGroupByKeyAsync(string groupKey)
        {
            return await _context.PermissionGroups
                .Include(pg => pg.Permissions)  // Include the related permissions
                .FirstOrDefaultAsync(pg => pg.Key == groupKey);
        }

        

        public async Task<List<TenantPermissionEntity>> GetPermissionsByKeysAsync(IEnumerable<string> keys)
        {
            return await _context.Permissions
                .Where(p => keys.Contains(p.Key))
                .ToListAsync();
        }

        // Add or update a permission group (if needed)
        public async Task UpdatePermissionGroupAsync(TenantPermissionGroupEntity permissionGroupEntity)
        {
            // Check if the permission group already exists in the context
            var existingGroup = await GetPermissionGroupByKeyAsync(permissionGroupEntity.Key);
            if (existingGroup != null)
            {
                // Update existing group
                existingGroup.Name = permissionGroupEntity.Name;
                _context.PermissionGroups.Update(existingGroup);
            }
            else
            {
                // Add new group
                await _context.PermissionGroups.AddAsync(permissionGroupEntity);
            }

            await _context.SaveChangesAsync();
        }

        // Delete a permission from the system
        public async Task DeletePermissionAsync(TenantPermissionEntity permissionEntity)
        {
            // Ensure the permission exists before attempting to delete
            var existingPermission = await GetPermissionByKeyAsync(permissionEntity.Key);
            if (existingPermission != null)
            {
                _context.Permissions.Remove(existingPermission);
                await _context.SaveChangesAsync();
            }
        }
    }
}
