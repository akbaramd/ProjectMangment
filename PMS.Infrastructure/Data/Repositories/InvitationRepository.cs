using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InvitationRepository : EfGenericRepository<ApplicationDbContext, Invitation>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Invitation?> GetInvitationByEmailAsync(string email)
    {
        return _context.Invitations.FirstOrDefaultAsync(invitation => invitation.Email == email);
    }

    public Task<List<Invitation>> GetInvitationsByStatusAsync(InvitationStatus status)
    {
        return _context.Invitations.Where(invitation => invitation.Status == status).ToListAsync();
    }

    public Task<List<Invitation>> GetInvitationsByTenantIdAsync(Guid tenantId)
    {
        return _context.Invitations.Where(invitation => invitation.TenantId == tenantId).ToListAsync();
    }
}