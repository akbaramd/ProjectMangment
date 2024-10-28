using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface ITenantSeeder
    {
        Task<Tenant> SeedTenantAsync(string name, string subdomain,PermissionsData data);
    }
}
