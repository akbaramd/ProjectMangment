using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TenantManagment.Repositories
{
    public interface ITenantMemberRepository : IGenericRepository<TenantMemberEntity>
    {
        
        Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId);
        Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId);
        Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId);
        Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId);
        
        Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId);

        Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, Guid tenantId);
   
    }
}
