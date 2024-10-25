using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantMemberRepository : EfGenericRepository<ApplicationDbContext, TenantMemberEntity>, ITenantMemberRepository
{
    public TenantMemberRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId)
    {
        return _context.TenantMember
            .Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
    }

    public Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        return _context.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.TenantId == tenantId).ToListAsync();
    }

    public Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId)
    {
        return _context.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.UserId == userId).ToListAsync();
    }

    public Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId)
    {
        return _context.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).AnyAsync(ut => ut.UserId == id && ut.TenantId == tenantId);
    }

    public Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId)
    {
        return _context.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(x => x.TenantId == tenantId).ToListAsync();
    }

    public Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, Guid tenantId)
    {
        return _context.TenantMember
            .Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.User.PhoneNumber == phoneNumber && ut.TenantId == tenantId);
    }

   
}