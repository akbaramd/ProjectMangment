using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IUserTenantSeeder
    {
        Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, string memberRole);
    }
}
