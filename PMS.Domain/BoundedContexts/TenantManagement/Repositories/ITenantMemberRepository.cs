using Bonyan.Layer.Domain.Abstractions;
using Bonyan.TenantManagement.Domain;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface ITenantMemberRepository : IRepository<TenantMemberEntity,Guid>
    {
        
        Task<TenantMemberEntity?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, TenantId tenantId);
        Task<List<TenantMemberEntity>> GetUsersByTenantIdAsync(Guid tenantId);
        Task<List<TenantMemberEntity>> GetTenantsByUserIdAsync(Guid userId);
        Task<bool> IsUserInTenantAsync(Guid id, TenantId tenantId);
        
        Task<List<TenantMemberEntity>> GetMembersByTenantIdAsync(TenantId tenantId);

        Task<TenantMemberEntity?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, TenantId tenantId);
   
    }
}
