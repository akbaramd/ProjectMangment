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

        public DatabaseSeeder(
            IRoleSeeder roleSeeder,
            IUserSeeder userSeeder,
            ITenantSeeder tenantSeeder,
            IUserTenantSeeder userTenantSeeder)
        {
            _roleSeeder = roleSeeder;
            _userSeeder = userSeeder;
            _tenantSeeder = tenantSeeder;
            _userTenantSeeder = userTenantSeeder;
        }

        public async Task SeedDefaultUserAsync()
        {
            var policyNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(c => c.GetAllPolicyNames())
                .Distinct();

            await _roleSeeder.SeedRoleAsync("Developer", policyNames);
            await _roleSeeder.SeedRoleAsync("User", []);

            var user = await _userSeeder.SeedUserAsync("akbarsafari00@gmail.com", "Akbar Ahmadi Saray", "09371770774", "Password123!");

            var tenant = await _tenantSeeder.SeedTenantAsync("Akbar AMD", "akbaramd");

            await _userTenantSeeder.SeedUserTenantAsync(user, tenant, UserTenantRole.Owner);
        }
    }
}
