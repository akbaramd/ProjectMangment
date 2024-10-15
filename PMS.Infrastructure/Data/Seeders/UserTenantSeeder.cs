using PMS.Domain.Entities;

namespace PMS.Infrastructure.Seeding
{
    public class UserTenantSeeder : IUserTenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTenantSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, UserTenantRole role)
        {
            var userTenant = _dbContext.UserTenants
                .FirstOrDefault(ut => ut.UserId == user.Id && ut.TenantId == tenant.Id);
            if (userTenant == null)
            {
                userTenant = new UserTenant(user, tenant, role);
                _dbContext.UserTenants.Add(userTenant);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
