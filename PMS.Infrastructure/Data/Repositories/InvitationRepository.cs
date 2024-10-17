using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class InvitationRepository : EfGenericRepository<ApplicationDbContext, Invitation>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Invitation?> GetInvitationByEmailAsync(string email)
    {
        return _context.Invitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == email);
    }

    public Task<List<Invitation>> GetInvitationsByStatusAsync(InvitationStatus status)
    {
        return _context.Invitations.Where(invitation => invitation.Status == status).ToListAsync();
    }

    public Task<List<Invitation>> GetInvitationsByTenantIdAsync(Guid tenantId)
    {
        return _context.Invitations.Where(invitation => invitation.TenantId == tenantId).ToListAsync();
    }

    public Task<Invitation?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId)
    {
        return _context.Invitations.FirstOrDefaultAsync(invitation => invitation.PhoneNumber == phoneNumber && invitation.TenantId == tenantId);
    }

    public IQueryable<Invitation> Query()
    {
        return _context.Invitations;
    }
}