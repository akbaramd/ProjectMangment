using PMS.Domain.Entities;

namespace PMS.Infrastructure.Seeding
{
    public interface IUserTenantSeeder
    {
        Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, UserTenantRole role);
    }
}
