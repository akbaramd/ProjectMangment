using PMS.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.Domain.Repositories
{
    public interface IInvitationRepository : IGenericRepository<Invitation>
    {
        Task<Invitation?> GetInvitationByEmailAsync(string email);
        Task<List<Invitation>> GetInvitationsByStatusAsync(InvitationStatus status);
        Task<List<Invitation>> GetInvitationsByTenantIdAsync(Guid tenantId);
    }
}
