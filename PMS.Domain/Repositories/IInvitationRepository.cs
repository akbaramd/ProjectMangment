using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface IInvitationRepository : IGenericRepository<Invitation>
    {
        Task<Invitation?> GetInvitationByEmailAsync(string email);
        Task<List<Invitation>> GetInvitationsByStatusAsync(InvitationStatus status);
        Task<List<Invitation>> GetInvitationsByTenantIdAsync(Guid tenantId);
        Task<Invitation?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId);
        IQueryable<Invitation> Query();
    }
}
