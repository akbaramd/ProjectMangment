using System.Security.Claims;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PMS.Infrastructure.Data.Seeders
{
    public class TenantSeeder : ITenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;

        public TenantSeeder(ApplicationDbContext dbContext, RoleManager<ApplicationRole> roleManager, IPermissionRepository permissionRepository)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
        }

        public async Task<Tenant> SeedTenantAsync(string name, string subdomain)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.Name == name);
            if (tenant == null)
            {
                tenant = new Tenant(name, subdomain);
                _dbContext.Tenants.Add(tenant);
                await _dbContext.SaveChangesAsync();

                // Create roles for the tenant
                await SeedRolesForTenantAsync(tenant);
            }
            return tenant;
        }

        private async Task SeedRolesForTenantAsync(Tenant tenant)
        {
            // Seed Owner role with full permissions
            var ownerRole = new ApplicationRole("Owner", tenantId:tenant.Id ,deletable: false, isSystemRole: false);
            await _roleManager.CreateAsync(ownerRole);
            await AssignFullPermissions(ownerRole);

            // Seed Manager role with full permissions (same as owner)
            var managerRole = new ApplicationRole("Manager", tenantId:tenant.Id ,deletable: false, isSystemRole: false);
            
            await _roleManager.CreateAsync(managerRole);
            await AssignFullPermissions(managerRole);

            // Seed Employee role with document and task read/write, and read-only for tenant/project
            var employeeRole = new ApplicationRole("Employee", tenantId:tenant.Id ,deletable: false, isSystemRole: false);;
            await _roleManager.CreateAsync(employeeRole);
            await AssignEmployeePermissions(employeeRole);

            // Seed Guest role with read-only permissions for documents, tasks, tenant, and project
            var guestRole = new ApplicationRole("Guest", tenantId:tenant.Id ,deletable: false, isSystemRole: false);
            await _roleManager.CreateAsync(guestRole);
            await AssignGuestPermissions(guestRole);
        }

        private async Task AssignFullPermissions(ApplicationRole role)
        {
            // Full permissions for Owner and Manager (Tenant, Project, Document, Task, Invitation)
            var permissions = await _permissionRepository.GetPermissionsByKeysAsync(new[]
            {
                "tenant:create", "tenant:read", "tenant:update", "tenant:delete",
                "project:create", "project:read", "project:update", "project:delete",
                "document:create", "document:read", "document:update", "document:delete",
                "task:create", "task:read", "task:update", "task:delete",
                "invitation:create", "invitation:read", "invitation:update", "invitation:delete"
            });

            // Assign all the permissions to the role
            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission.Key));
            }
            
            await _roleManager.UpdateAsync(role);
        }

        private async Task AssignEmployeePermissions(ApplicationRole role)
        {
            // Employee permissions: Full access to Document and Task, Read-only for Tenant and Project
            var permissions = await _permissionRepository.GetPermissionsByKeysAsync(new[]
            {
                "document:create", "document:read", "document:update", "document:delete",
                "task:create", "task:read", "task:update", "task:delete",
                "tenant:read",  // Read-only for Tenant
                "project:read"  // Read-only for Project
            });

            // Assign all the permissions to the role
            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission.Key));
            }
            
            await _roleManager.UpdateAsync(role);
        }

        private async Task AssignGuestPermissions(ApplicationRole role)
        {
            // Guest permissions: Read-only access to Documents, Tasks, Tenant, and Project
            var permissions = await _permissionRepository.GetPermissionsByKeysAsync(new[]
            {
                "document:read",   // Read-only for Documents
                "task:read",       // Read-only for Tasks
                "tenant:read",     // Read-only for Tenant
                "project:read"     // Read-only for Project
            });

            // Assign all the permissions to the role
            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission.Key));
            }
            
            await _roleManager.UpdateAsync(role);
        }
    }
}
