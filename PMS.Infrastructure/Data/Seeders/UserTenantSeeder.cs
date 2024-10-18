using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserTenantSeeder : IUserTenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTenantSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, TenantMemberRole memberRole)
        {
            var userTenant = _dbContext.TenantMember
                .FirstOrDefault(ut => ut.UserId == user.Id && ut.TenantId == tenant.Id);
            if (userTenant == null)
            {
                userTenant = new TenantMember(user, tenant, memberRole);
                _dbContext.TenantMember.Add(userTenant);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
