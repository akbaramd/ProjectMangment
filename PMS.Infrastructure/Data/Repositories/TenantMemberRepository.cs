using Bonyan.Layer.Domain;
using Bonyan.TenantManagement.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class TenantMemberRepository : EfCoreRepository< TenantMemberEntity,Guid,ApplicationDbContext>, ITenantMemberRepository
{

    public async Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, TenantId tenantId)
    {
        return  await (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId.Value);
    }

    public async Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        return await  (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).Where(ut => ut.TenantId == tenantId).ToListAsync();
    }

    public async Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId)
    {
        return  await (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).Where(ut => ut.UserId == userId).ToListAsync();
    }

    public async Task<bool> IsUserInTenantAsync(Guid id, TenantId tenantId)
    {
        return   await (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).AnyAsync(ut => ut.UserId == id && ut.TenantId == tenantId.Value);
    }

    public Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(TenantId tenantId)
    {
        return  await (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).Where(x => x.TenantId == tenantId.Value).ToListAsync();
    }

    public async Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, TenantId tenantId)
    {
        return  await (await GetDbContextAsync()).TenantMember
            .Include(x=>x.Roles)
            .ThenInclude(x=>x.Permissions)
            .ThenInclude(x=>x.Group)
            .Include(x=>x.UserEntity).FirstOrDefaultAsync(ut => ut.UserEntity.PhoneNumber == phoneNumber && ut.TenantId == tenantId.Value);
    }


    public TenantMemberRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}