using PMS.Domain.BoundedContexts.TenantManagment;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface ITenantSeeder
    {
        Task<TenantEntity> SeedTenantAsync(string name, string subdomain,PermissionsData data);
    }
}
