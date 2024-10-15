
using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using System.Security.Claims;

namespace PMS.Infrastructure.Seeding
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole(roleName, deletable: false);
                await _roleManager.CreateAsync(role);

                foreach (var policy in policyNames)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, policy));
                }
            }
        }
    }
}
