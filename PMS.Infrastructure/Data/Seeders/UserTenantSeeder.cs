using PMS.Infrastructure.Data.Seeders.Absractions;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserTenantSeeder : IUserTenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTenantSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUserTenantAsync(ApplicationUser user, TenantEntity tenantEntity, string roleName)
        {
            // Retrieve the tenantEntity roles for this tenantEntity
            var tenantRole = await _dbContext.TenantRole
                .Where(r => r.TenantId == tenantEntity.Id && r.Title == roleName)
                .FirstOrDefaultAsync();

            
            // Check if user is already assigned to this tenantEntity
            var userTenant = await _dbContext.TenantMember
                .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TenantId == tenantEntity.Id);

            if (userTenant == null)
            {
                // Create the TenantMember entry if it doesn't exist
                userTenant = new TenantMemberEntity(user, tenantEntity);
                userTenant.AddRole(tenantRole);
                _dbContext.TenantMember.Add(userTenant);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
