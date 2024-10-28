using Bonyan.DomainDrivenDesign.Domain;
using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantMemberRepository : EfCoreRepository< TenantMemberEntity,Guid,ApplicationDbContext>, ITenantMemberRepository
{

    public Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, TenantId tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId.Value);
    }

    public Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.TenantId == tenantId).ToListAsync();
    }

    public Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(ut => ut.UserId == userId).ToListAsync();
    }

    public Task<bool> IsUserInTenantAsync(Guid id, TenantId tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).AnyAsync(ut => ut.UserId == id && ut.TenantId == tenantId.Value);
    }

    public Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(TenantId tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).Where(x => x.TenantId == tenantId.Value).ToListAsync();
    }

    public Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, TenantId tenantId)
    {
        return _dbContext.TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.User).FirstOrDefaultAsync(ut => ut.User.PhoneNumber == phoneNumber && ut.TenantId == tenantId.Value);
    }


    public TenantMemberRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}