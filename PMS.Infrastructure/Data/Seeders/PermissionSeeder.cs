using System.IO;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Repositories;

namespace PMS.Infrastructure.Data.Seeders
{
    public class PermissionSeeder : IPermissionSeeder
    {
        private readonly IRoleRepository _roleManager;
        private readonly ApplicationDbContext _context;

        public PermissionSeeder(IRoleRepository roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<PermissionsData> SeedFromJsonAsync(string jsonFilePath)
        {
            var jsonString = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialize JSON to get permissions data
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            var permissionsData = JsonSerializer.Deserialize<PermissionsData>(jsonString, options);
            if (permissionsData == null) throw new Exception("Failed to load permissions data from JSON.");

            // Seed all permission groups and permissions
            foreach (var group in permissionsData.PermissionGroups)
            {
                await SeedGroupAsync(group.Key, group.Name);

                foreach (var permission in group.Permissions)
                {
                    await SeedAsync(group.Key, permission.Key, permission.Name);
                }
            }

            // Remove permissions that are not in the JSON file
            await RemoveObsoletePermissionsAsync(permissionsData.PermissionGroups);

            return permissionsData;
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
                var permission = new ApplicationPermission
                {
                    Key = key,
                    GroupKey = groupKey,
                    Name = title
                };

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();
            }

        }

        // Seeds a permission group, useful for categorizing permissions
        public async Task SeedGroupAsync(string key, string title)
        {
            // Check if the group exists in the database
            var existingGroup = await _context.PermissionGroups.FirstOrDefaultAsync(pg => pg.Key == key);

            if (existingGroup == null)
            {
                var permissionGroup = new ApplicationPermissionGroup
                {
                    Key = key,
                    Name = title
                };

                _context.PermissionGroups.Add(permissionGroup);
                await _context.SaveChangesAsync();
            }
        }

        // Removes permissions that are not in the JSON file
        private async Task RemoveObsoletePermissionsAsync(List<PermissionGroupDto> jsonPermissionGroups)
        {
            // Collect all permission keys from the JSON data
            var jsonPermissionKeys = jsonPermissionGroups
                .SelectMany(g => g.Permissions)
                .Select(p => p.Key)
                .ToHashSet();

            // Fetch all existing permissions from the database
            var existingPermissions = await _context.Permissions.ToListAsync();

            // Remove permissions that are not in the JSON file
            foreach (var permission in existingPermissions)
            {
                if (!jsonPermissionKeys.Contains(permission.Key))
                {
                    _context.Permissions.Remove(permission);

                }
            }

            await _context.SaveChangesAsync();
            
       
        }
    }

    // Define the model for deserializing the JSON data
    public class PermissionsData
    {
        public List<PermissionGroupDto> PermissionGroups { get; set; }
        public List<RoleData> Roles { get; set; }
    }
    public class RoleData
    {
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }
    }
    public class PermissionGroupDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class PermissionDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
