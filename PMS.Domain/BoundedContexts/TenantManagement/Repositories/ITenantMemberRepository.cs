using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface ITenantMemberRepository : IRepository<TenantMemberEntity,Guid>
    {
        
        Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId);
        Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId);
        Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId);
        Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId);
        
        Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(Guid tenantId);

        Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, Guid tenantId);
   
    }
}
