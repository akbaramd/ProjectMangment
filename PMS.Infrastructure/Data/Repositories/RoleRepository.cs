using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class RoleRepository : EfGenericRepository<ApplicationDbContext, TenantRole>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<TenantRole?> GetRoleByNameAsync(string roleName)
    {
        return _context.TenantRole.FirstOrDefaultAsync(r => r.Key == roleName);
    }

    public List<TenantRole> GetByTenantId(Guid tenantId)
    {
        return _context.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).Where(x => x.TenantId == tenantId).ToList();
    }
    
    public new Task<TenantRole?> GetByIdAsync(Guid tenantId)
    {
        return _context.TenantRole.Include(x=>x.Permissions).ThenInclude(x=>x.Group).FirstOrDefaultAsync(x => x.Id == tenantId);
    }
}