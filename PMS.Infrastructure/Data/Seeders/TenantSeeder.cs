using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class TenantSeeder : ITenantSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public TenantSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tenant> SeedTenantAsync(string name, string subdomain)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.Name == name);
            if (tenant == null)
            {
                tenant = new Tenant(name, subdomain);
                _dbContext.Tenants.Add(tenant);
                await _dbContext.SaveChangesAsync();
            }
            return tenant;
        }
    }
}
