using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IUserTenantSeeder
    {
        Task SeedUserTenantAsync(ApplicationUser user, TenantEntity tenantEntity, string memberRole);
    }
}
