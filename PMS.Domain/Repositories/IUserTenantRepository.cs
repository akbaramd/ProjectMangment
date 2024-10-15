using PMS.Domain.Entities;
using SharedKernel.Domain;

namespace PMS.Domain.Repositories
{
    public interface IUserTenantRepository : IGenericRepository<UserTenant>
    {
        Task<UserTenant?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId);
        Task<List<UserTenant>> GetUsersByTenantIdAsync(Guid tenantId);
        Task<List<UserTenant>> GetTenantsByUserIdAsync(Guid userId);
        Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId);
    }
}
