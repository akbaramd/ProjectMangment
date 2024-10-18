using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using PMS.Infrastructure.Extensions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class DatabaseSeeder
    {
        private readonly IRoleSeeder _roleSeeder;
        private readonly IUserSeeder _userSeeder;
        private readonly ITenantSeeder _tenantSeeder;
        private readonly IUserTenantSeeder _userTenantSeeder;
        private readonly IPermissionSeeder _permissionSeeder;  // Injecting the permission seeder

        public DatabaseSeeder(
            IRoleSeeder roleSeeder,
            IUserSeeder userSeeder,
            ITenantSeeder tenantSeeder,
            IUserTenantSeeder userTenantSeeder,
            IPermissionSeeder permissionSeeder)  // Inject IPermissionSeeder
        {
            _roleSeeder = roleSeeder;
            _userSeeder = userSeeder;
            _tenantSeeder = tenantSeeder;
            _userTenantSeeder = userTenantSeeder;
            _permissionSeeder = permissionSeeder;  // Assign it here
        }

        public async Task SeedDefaultUserAsync()
        {
            // Seed permissions for CRUD operations
            await SeedPermissionsAsync();

            // Seed a default user
            var user = await _userSeeder.SeedUserAsync("akbarsafari00@gmail.com", "Akbar Ahmadi Saray", "09371770774", "Password123!");

            // Seed a default tenant
            var tenant = await _tenantSeeder.SeedTenantAsync("Akbar AMD", "akbaramd");

            // Assign the user as the owner of the tenant
            await _userTenantSeeder.SeedUserTenantAsync(user, tenant, TenantMemberRole.Owner);
        }

        // Seeding CRUD system permissions for multiple modules including documents, tasks, tenants, and invitations
        private async Task SeedPermissionsAsync()
        {
            // Define the permission groups and permissions for each entity
            var permissionGroups = new List<(string GroupKey, string GroupTitle, List<(string Key, string Title)> Permissions)>
            {
                ("user", "User Operations", new List<(string, string)>
                {
                    ("create", "Create User"),
                    ("read", "Read User"),
                    ("update", "Update User"),
                    ("delete", "Delete User")
                }),
                ("tenant", "Tenant Operations", new List<(string, string)>
                {
                    ("create", "Create Tenant"),
                    ("read", "Read Tenant"),
                    ("update", "Update Tenant"),
                    ("delete", "Delete Tenant")
                }),
                ("project", "Project Operations", new List<(string, string)>
                {
                    ("create", "Create Project"),
                    ("read", "Read Project"),
                    ("update", "Update Project"),
                    ("delete", "Delete Project")
                }),
                ("document", "Document Operations", new List<(string, string)>
                {
                    ("create", "Create Document"),
                    ("read", "Read Document"),
                    ("update", "Update Document"),
                    ("delete", "Delete Document")
                }),
                ("task", "Task Operations", new List<(string, string)>
                {
                    ("create", "Create Task"),
                    ("read", "Read Task"),
                    ("update", "Update Task"),
                    ("delete", "Delete Task")
                }),
                ("invitation", "Invitation Operations", new List<(string, string)>
                {
                    ("create", "Create Invitation"),
                    ("read", "Read Invitation"),
                    ("update", "Update Invitation"),
                    ("delete", "Delete Invitation")
                })
            };

            // List to store all permissions to be assigned to Developer
            var allPermissions = new List<string>();

            // Seed each permission group and its permissions
            foreach (var (groupKey, groupTitle, permissions) in permissionGroups)
            {
                // Seed the permission group
                await _permissionSeeder.SeedGroupAsync(groupKey, groupTitle);

                // Seed each permission in the group
                foreach (var (key, title) in permissions)
                {
                    await _permissionSeeder.SeedAsync(groupKey, $"{groupKey}:{key}", title);
                    allPermissions.Add($"{groupKey}:{key}");
                }
            }

            // Assign all permissions to Developer role
            await _roleSeeder.SeedRoleAsync("Developer", allPermissions);  // Developer gets full access to all permissions
        }
    }
}
