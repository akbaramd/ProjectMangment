using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;
using Microsoft.AspNetCore.Identity;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserTenantSeeder : IUserTenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserTenantSeeder(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedUserTenantAsync(ApplicationUser user, Tenant tenant, TenantMemberRole memberRole)
        {
            // Check if user is already assigned to this tenant
            var userTenant = _dbContext.TenantMember
                .FirstOrDefault(ut => ut.UserId == user.Id && ut.TenantId == tenant.Id);

            if (userTenant == null)
            {
                // Create the TenantMember entry if it doesn't exist
                userTenant = new TenantMember(user, tenant, memberRole);
                _dbContext.TenantMember.Add(userTenant);
                await _dbContext.SaveChangesAsync();
            }

            // Assign the role based on TenantMemberRole
            var roleName = GetRoleNameForMemberRole(memberRole);
            roleName = $"{roleName}:{tenant.Id}";
            // Check if user already has this role for this tenant
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                // Add role to user
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        // A utility function that maps TenantMemberRole to role names
        private string GetRoleNameForMemberRole(TenantMemberRole memberRole)
        {
            return memberRole switch
            {
                TenantMemberRole.Manager => "Manager",
                TenantMemberRole.Owner => "Owner",
                TenantMemberRole.Employee => "Employee",
                TenantMemberRole.Guest => "Guest",
                _ => throw new ArgumentOutOfRangeException(nameof(memberRole), $"Unsupported role: {memberRole}")
            };
        }
    }
}
