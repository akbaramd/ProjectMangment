using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TenantManagment.Repositories
{
    public interface IInvitationRepository : IGenericRepository<ProjectInvitationEntity>
    {
        Task<ProjectInvitationEntity?> GetInvitationByEmailAsync(string email);
        Task<List<ProjectInvitationEntity>> GetInvitationsByStatusAsync(InvitationStatus status);
        Task<List<ProjectInvitationEntity>> GetInvitationsByTenantIdAsync(Guid tenantId);
        Task<ProjectInvitationEntity?> GetInvitationByPhoneNumberAndTenantAsync(string phoneNumber, Guid tenantId);
        IQueryable<ProjectInvitationEntity> Query();
    }
}
