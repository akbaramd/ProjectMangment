using PMS.Infrastructure.Data.Seeders.Absractions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly ApplicationDbContext _context;  // DbContext to query permissions

        public RoleSeeder( ApplicationDbContext context)
        {
            _context = context;  // Inject DbContext
        }

        public async Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames)
        {
     
        }
    }
}