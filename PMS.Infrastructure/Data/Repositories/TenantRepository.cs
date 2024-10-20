using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantRepository : EfGenericRepository<ApplicationDbContext, Tenant>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Tenant?> GetTenantBySubdomainAsync(string subdomain)
    {
        return _context.Tenants.Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .FirstOrDefaultAsync(t => t.Subdomain == subdomain);
    }

    public Task<Tenant?> GetTenantByNameAsync(string name)
    {
        return _context.Tenants.FirstOrDefaultAsync(t => t.Name == name);
    }
}