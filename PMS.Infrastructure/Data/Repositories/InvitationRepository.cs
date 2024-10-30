using Bonyan.Layer.Domain;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class InvitationRepository : EfCoreRepository<ProjectInvitationEntity,Guid,ApplicationDbContext>, IInvitationRepository
{

    public Task<ProjectInvitationEntity?> GetInvitationByEmailAsync(string email)
    {
        return  await (await GetDbContextAsync()).TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == email);
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByStatusAsync(InvitationStatus status)
    {
        return  await (await GetDbContextAsync()).TenantInvitations.Where(invitation => invitation.Status == status).ToListAsync();
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByTenantIdAsync(Guid tenantId)
    {
        return  await (await GetDbContextAsync()).TenantInvitations.Where(invitation => invitation.TenantId == tenantId).ToListAsync();
    }

    public Task<ProjectInvitationEntity?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId)
    {
        return  await (await GetDbContextAsync()).TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == phoneNumber && invitation.TenantId == tenantId);
    }

    public IQueryable<ProjectInvitationEntity> Query()
    {
        return  await (await GetDbContextAsync()).TenantInvitations;
    }

    public InvitationRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}