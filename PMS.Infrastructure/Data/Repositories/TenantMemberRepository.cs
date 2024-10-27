using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantMemberRepository : EfCoreRepository< TenantMemberEntity,Guid,ApplicationDbContext>, ITenantMemberRepository
{

    public Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
    }

    public Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        return _dbContext.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.TenantId == tenantId).ToListAsync();
    }

    public Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId)
    {
        return _dbContext.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.UserId == userId).ToListAsync();
    }

    public Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId)
    {
        return _dbContext.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).AnyAsync(ut => ut.UserId == id && ut.TenantId == tenantId);
    }

    public Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId)
    {
        return _dbContext.TenantMember.Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(x => x.TenantId == tenantId).ToListAsync();
    }

    public Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, Guid tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Tenant)
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.User.PhoneNumber == phoneNumber && ut.TenantId == tenantId);
    }


    public TenantMemberRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}