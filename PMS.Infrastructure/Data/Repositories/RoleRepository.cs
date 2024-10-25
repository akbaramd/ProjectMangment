using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class RoleRepository : EfGenericRepository<ApplicationDbContext, TenantRoleEntity>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<TenantRoleEntity?> GetRoleByNameAsync(string roleName)
    {
        return _context.TenantRole.FirstOrDefaultAsync(r => r.Key == roleName);
    }

    public List<TenantRoleEntity> GetByTenantId(Guid tenantId)
    {
        return _context.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).Where(x => x.TenantId == tenantId).ToList();
    }
    
    public new Task<TenantRoleEntity?> GetByIdAsync(Guid tenantId)
    {
        return _context.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).FirstOrDefaultAsync(x => x.Id == tenantId);
    }
}