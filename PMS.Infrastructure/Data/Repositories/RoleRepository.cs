using Bonyan.DomainDrivenDesign.Domain;
using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class RoleRepository : EfCoreRepository< TenantRoleEntity,Guid,ApplicationDbContext>, IRoleRepository
{

    public Task<TenantRoleEntity?> GetRoleByNameAsync(string roleName)
    {
        return _dbContext.TenantRole.FirstOrDefaultAsync(r => r.Key == roleName);
    }

    public List<TenantRoleEntity> GetByTenantId(TenantId tenantId)
    {
        return _dbContext.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).Where(x => x.TenantId == tenantId.Value).ToList();
    }
    
    public new Task<TenantRoleEntity?> GetByIdAsync(Guid tenantId)
    {
        return _dbContext.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).FirstOrDefaultAsync(x => x.Id == tenantId);
    }

    public RoleRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}