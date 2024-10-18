using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface ITenantMemberRepository : IGenericRepository<TenantMember>
    {
        
        Task<TenantMember?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId);
        Task<List<TenantMember>> GetUsersByTenantIdAsync(Guid tenantId);
        Task<List<TenantMember>> GetTenantsByUserIdAsync(Guid userId);
        Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId);
        
        Task<List<TenantMember>> GetMembersByTenantIdAsync(Guid tenantId);

        Task<TenantMember?> GetTenantMemberByPhoneNumberAsync(string phoneNumber, Guid tenantId);
    }
}
