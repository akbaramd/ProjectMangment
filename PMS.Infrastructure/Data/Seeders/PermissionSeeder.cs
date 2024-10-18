using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Seeders
{
    public class PermissionSeeder : IPermissionSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public PermissionSeeder(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // Seeds a permission under a specific group for a given role
        public async Task SeedAsync(string groupKey, string key, string title)
        {
            // Check if the permission exists in the database
            var existingPermission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Key == key && p.GroupKey == groupKey);
            
            // If the permission does not exist, create a new one
            if (existingPermission == null)
            {
                var permission = new ApplicationPermission()
                {
                    Key = key,
                    GroupKey = groupKey,
                    Name = title
                };

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();
            }

            // Now assign this permission to specific roles
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                // Check if the role already has the claim (permission)
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var hasClaim = roleClaims.Any(c => c.Type == "Permission" && c.Value == $"{groupKey}:{key}");

                // If the claim does not exist, add it to the role
                if (!hasClaim)
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"{groupKey}:{key}"));
                }
            }
        }

        // Seeds a permission group, useful for categorizing permissions
        public async Task SeedGroupAsync(string key, string title)
        {
            // Check if the group exists in the database
            var existingGroup = await _context.PermissionGroups.FirstOrDefaultAsync(pg => pg.Key == key);

            if (existingGroup == null)
            {
                var permissionGroup = new ApplicationPermissionGroup()
                {
                    Key = key,
                    Name = title
                };

                _context.PermissionGroups.Add(permissionGroup);
                await _context.SaveChangesAsync();
            }
        }
    }
}
