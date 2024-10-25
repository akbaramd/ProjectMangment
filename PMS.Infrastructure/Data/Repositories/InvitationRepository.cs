using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class InvitationRepository : EfGenericRepository<ApplicationDbContext, ProjectInvitationEntity>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<ProjectInvitationEntity?> GetInvitationByEmailAsync(string email)
    {
        return _context.TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == email);
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByStatusAsync(InvitationStatus status)
    {
        return _context.TenantInvitations.Where(invitation => invitation.Status == status).ToListAsync();
    }

    public Task<List<ProjectInvitationEntity>> GetInvitationsByTenantIdAsync(Guid tenantId)
    {
        return _context.TenantInvitations.Where(invitation => invitation.TenantId == tenantId).ToListAsync();
    }

    public Task<ProjectInvitationEntity?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId)
    {
        return _context.TenantInvitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == phoneNumber && invitation.TenantId == tenantId);
    }

    public IQueryable<ProjectInvitationEntity> Query()
    {
        return _context.TenantInvitations;
    }
}