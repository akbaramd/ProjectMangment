using System.Security.Claims;
using PMS.Infrastructure.Data.Seeders.Absractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;

namespace PMS.Infrastructure.Data.Seeders
{
    public class TenantSeeder : ITenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRoleRepository _roleManager;
        private readonly IPermissionRepository _permissionRepository;

        public TenantSeeder(ApplicationDbContext dbContext, IRoleRepository roleManager, IPermissionRepository permissionRepository)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
        }

        // The SeedTenantAsync method accepts PermissionsData and syncs roles and permissions accordingly
        public async Task<TenantEntity> SeedTenantAsync(string name, string subdomain, PermissionsData permissionsData)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Name == name);
            
            if (tenant != null)
            {
                // Update roles and permissions for the existing tenantEntity
                await SyncRolesForTenantAsync(tenant, permissionsData);
            }
            else
            {
                // Create a new tenantEntity and seed the roles
                tenant = new TenantEntity(name, subdomain);
                _dbContext.Tenants.Add(tenant);
                await _dbContext.SaveChangesAsync();

                // Create roles for the new tenantEntity with the provided permissions data
                await SeedRolesForTenantAsync(tenant, permissionsData);
            }

            return tenant;
        }

        // Synchronize the roles for an existing tenantEntity
        private async Task SyncRolesForTenantAsync(TenantEntity tenantEntity, PermissionsData permissionsData)
        {
            var existingRoles =  _roleManager.GetByTenantId(tenantEntity.Id);

            // For each role in PermissionsData, either update or create the role
            foreach (var roleData in permissionsData.Roles)
            {
                var existingRole = existingRoles.FirstOrDefault(r => r.TenantId == tenantEntity.Id && r.Key == roleData.RoleName.ToLower().Replace(" ","-"));
                
                if (existingRole != null)
                {
                    // Role exists in both the database and the permissions data, sync permissions
                    await ReassignPermissionsForRole(existingRole, roleData.Permissions);
                }
                else
                {
                    // Role does not exist in the database, create it and sync permissions
                    var newRole = new TenantRoleEntity(roleData.RoleName, tenantEntity, deletable: false, isSystemRole: false);
                    await _roleManager.AddAsync(newRole);
                    await ReassignPermissionsForRole(newRole, roleData.Permissions);
                }
            }

            // Roles that exist in the database but not in PermissionsData should be left untouched
        }

        // Seed roles for a newly created tenantEntity
        private async Task SeedRolesForTenantAsync(TenantEntity tenantEntity, PermissionsData permissionsData)
        {
            foreach (var roleData in permissionsData.Roles)
            {
                // Create the role
                var role = new TenantRoleEntity(roleData.RoleName,tenantEntity, deletable: false, isSystemRole: false);
                await _roleManager.AddAsync(role);

                // Assign permissions from PermissionsData
                await ReassignPermissionsForRole(role, roleData.Permissions);
            }
        }

        // This method removes all permissions from the role and reassigns the updated list
        private async Task ReassignPermissionsForRole(TenantRoleEntity roleEntity, List<string> newPermissions)
        {
            // Remove all current permissions
            var existingPermissions = roleEntity.Permissions.Select(p => p.Key).ToList();
            
            foreach (var permissionKey in existingPermissions)
            {
                var permissionToRemove = roleEntity.Permissions.FirstOrDefault(p => p.Key == permissionKey);
                if (permissionToRemove != null)
                {
                    roleEntity.Permissions.Remove(permissionToRemove);
                }
            }

            // Now reassign the new permissions
            foreach (var permissionKey in newPermissions)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission != null)
                {
                    roleEntity.Permissions.Add(permission);
                }
            }

            // Save role updates
            await _roleManager.UpdateAsync(roleEntity);
        }
    }


}
