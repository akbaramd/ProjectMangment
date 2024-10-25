using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantRepository : EfGenericRepository<ApplicationDbContext, TenantEntity>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<TenantEntity?> GetTenantBySubdomainAsync(string subdomain)
    {
        return _context.Tenants.Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .FirstOrDefaultAsync(t => t.Subdomain == subdomain);
    }

    public Task<TenantEntity?> GetTenantByNameAsync(string name)
    {
        return _context.Tenants.FirstOrDefaultAsync(t => t.Name == name);
    }
}