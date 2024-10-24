using System.Security.Claims;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PMS.Domain.Repositories;

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
        public async Task<Tenant> SeedTenantAsync(string name, string subdomain, PermissionsData permissionsData)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Name == name);
            
            if (tenant != null)
            {
                // Update roles and permissions for the existing tenant
                await SyncRolesForTenantAsync(tenant, permissionsData);
            }
            else
            {
                // Create a new tenant and seed the roles
                tenant = new Tenant(name, subdomain);
                _dbContext.Tenants.Add(tenant);
                await _dbContext.SaveChangesAsync();

                // Create roles for the new tenant with the provided permissions data
                await SeedRolesForTenantAsync(tenant, permissionsData);
            }

            return tenant;
        }

        // Synchronize the roles for an existing tenant
        private async Task SyncRolesForTenantAsync(Tenant tenant, PermissionsData permissionsData)
        {
            var existingRoles =  _roleManager.GetByTenantId(tenant.Id);

            // For each role in PermissionsData, either update or create the role
            foreach (var roleData in permissionsData.Roles)
            {
                var existingRole = existingRoles.FirstOrDefault(r => r.TenantId == tenant.Id && r.Key == roleData.RoleName.ToLower().Replace(" ","-"));
                
                if (existingRole != null)
                {
                    // Role exists in both the database and the permissions data, sync permissions
                    await ReassignPermissionsForRole(existingRole, roleData.Permissions);
                }
                else
                {
                    // Role does not exist in the database, create it and sync permissions
                    var newRole = new TenantRole(roleData.RoleName, tenantId: tenant.Id, deletable: false, isSystemRole: false);
                    await _roleManager.AddAsync(newRole);
                    await ReassignPermissionsForRole(newRole, roleData.Permissions);
                }
            }

            // Roles that exist in the database but not in PermissionsData should be left untouched
        }

        // Seed roles for a newly created tenant
        private async Task SeedRolesForTenantAsync(Tenant tenant, PermissionsData permissionsData)
        {
            foreach (var roleData in permissionsData.Roles)
            {
                // Create the role
                var role = new TenantRole(roleData.RoleName, tenantId: tenant.Id, deletable: false, isSystemRole: false);
                await _roleManager.AddAsync(role);

                // Assign permissions from PermissionsData
                await ReassignPermissionsForRole(role, roleData.Permissions);
            }
        }

        // This method removes all permissions from the role and reassigns the updated list
        private async Task ReassignPermissionsForRole(TenantRole role, List<string> newPermissions)
        {
            // Remove all current permissions
            var existingPermissions = role.Permissions.Select(p => p.Key).ToList();
            
            foreach (var permissionKey in existingPermissions)
            {
                var permissionToRemove = role.Permissions.FirstOrDefault(p => p.Key == permissionKey);
                if (permissionToRemove != null)
                {
                    role.Permissions.Remove(permissionToRemove);
                }
            }

            // Now reassign the new permissions
            foreach (var permissionKey in newPermissions)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission != null)
                {
                    role.Permissions.Add(permission);
                }
            }

            // Save role updates
            await _roleManager.UpdateAsync(role);
        }
    }


}
