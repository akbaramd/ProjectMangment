using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using PMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Seeders
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;  // DbContext to query permissions

        public RoleSeeder(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;  // Inject DbContext
        }

        public async Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames)
        {
            // Check if the role already exists
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                // Create the role if it doesn't exist
                var role = new ApplicationRole(roleName, deletable: false, isSystemRole: true);

                // Retrieve all permissions from the database that match the policyNames
                var permissions = await _context.Permissions
                    .Where(p => policyNames.Contains(p.Key))
                    .ToListAsync();

                // Add permissions to the role
                foreach (var permission in permissions)
                {
                    role.Permissions.Add(permission);
                }

                // Create the role in the database
                await _roleManager.CreateAsync(role);

                // Add claims (permissions) to the role
                foreach (var permission in permissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, permission.Key));
                }
            }
        }
    }
}