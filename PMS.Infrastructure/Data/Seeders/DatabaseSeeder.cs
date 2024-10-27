using PMS.Infrastructure.Data.Seeders.Absractions;

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
            IUserSeeder userSeeder,
            ITenantSeeder tenantSeeder,
            IUserTenantSeeder userTenantSeeder,
            IPermissionSeeder permissionSeeder)  // Inject IPermissionSeeder
        {
            _userSeeder = userSeeder;
            _tenantSeeder = tenantSeeder;
            _userTenantSeeder = userTenantSeeder;
            _permissionSeeder = permissionSeeder;  // Assign it here
        }

        public async Task SeedDefaultUserAsync()
        {
            // Seed permissions for CRUD operations
           var pd = await SeedPermissionsAsync();

            // Seed a default user
            var user = await _userSeeder.SeedUserAsync("akbarsafari00@gmail.com", "Akbar Ahmadi Saray", "09371770774", "Password123!");

            // Seed a default tenantEntity
            var tenant = await _tenantSeeder.SeedTenantAsync("Akbar AMD", "akbaramd",pd);

            // Assign the user as the owner of the tenantEntity
            await _userTenantSeeder.SeedUserTenantAsync(user, tenant, "Owner");
        }

        // Seeding CRUD system permissions for multiple modules including documents, tasks, tenants, and invitations
        private async Task<PermissionsData> SeedPermissionsAsync()
        {
           return await _permissionSeeder.SeedFromJsonAsync("./permissions.json");
        }
    }
}
