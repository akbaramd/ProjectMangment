using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class InvitationRepository : EfCoreRepository<ProjectInvitationEntity,Guid,ApplicationDbContext>, IInvitationRepository
{

    public Task<ProjectInvitationEntity?> GetInvitationByEmailAsync(string email)
    {
        return _dbContext.TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == email);
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByStatusAsync(InvitationStatus status)
    {
        return _dbContext.TenantInvitations.Where(invitation => invitation.Status == status).ToListAsync();
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByTenantIdAsync(Guid tenantId)
    {
        return _dbContext.TenantInvitations.Where(invitation => invitation.TenantId == tenantId).ToListAsync();
    }

    public Task<ProjectInvitationEntity?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId)
    {
        return _dbContext.TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == phoneNumber && invitation.TenantId == tenantId);
    }

    public IQueryable<ProjectInvitationEntity> Query()
    {
        return _dbContext.TenantInvitations;
    }

    public InvitationRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}