using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface ITenantSeeder
    {
        Task<Tenant> SeedTenantAsync(string name, string subdomain);
    }
}
