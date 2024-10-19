using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserTenantSeeder : IUserTenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTenantSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, string roleName)
        {
            // Retrieve the tenant roles for this tenant
            var tenantRole = await _dbContext.TenantRole
                .Where(r => r.TenantId == tenant.Id && r.Title == roleName)
                .FirstOrDefaultAsync();

            
            // Check if user is already assigned to this tenant
            var userTenant = await _dbContext.TenantMember
                .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TenantId == tenant.Id);

            if (userTenant == null)
            {
                // Create the TenantMember entry if it doesn't exist
                userTenant = new TenantMember(user, tenant);
                userTenant.AddRole(tenantRole);
                _dbContext.TenantMember.Add(userTenant);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
