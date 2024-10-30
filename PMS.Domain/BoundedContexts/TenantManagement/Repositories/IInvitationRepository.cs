using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface IInvitationRepository : IRepository<ProjectInvitationEntity,Guid>
    {
        Task<ProjectInvitationEntity?> GetInvitationByEmailAsync(string email);
        Task<List<ProjectInvitationEntity>> GetInvitationsByStatusAsync(InvitationStatus status);
        Task<List<ProjectInvitationEntity>> GetInvitationsByTenantIdAsync(Guid tenantId);
        Task<ProjectInvitationEntity?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId);
        IQueryable<ProjectInvitationEntity> Query();
    }
}
