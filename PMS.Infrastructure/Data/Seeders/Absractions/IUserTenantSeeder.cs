using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IUserTenantSeeder
    {
        Task SeedUserTenantAsync(ApplicationUser user, Tenant tenantEntity, string memberRole);
    }
}
