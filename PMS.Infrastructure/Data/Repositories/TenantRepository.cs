using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantRepository : EfCoreRepository< TenantEntity,Guid,ApplicationDbContext>, ITenantRepository
{

    public Task<TenantEntity?> GetTenantBySubdomainAsync(string subdomain)
    {
        return _dbContext.Tenants.Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .FirstOrDefaultAsync(t => t.Subdomain == subdomain);
    }

    public Task<TenantEntity?> GetTenantByNameAsync(string name)
    {
        return _dbContext.Tenants.FirstOrDefaultAsync(t => t.Name == name);
    }

    public TenantRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}