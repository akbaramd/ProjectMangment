using PMS.Domain.Entities;

namespace PMS.Infrastructure.Seeding
{
    public interface ITenantSeeder
    {
        Task<Tenant> SeedTenantAsync(string name, string subdomain);
    }
}
