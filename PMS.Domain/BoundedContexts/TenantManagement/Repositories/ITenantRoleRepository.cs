using Bonyan.Layer.Domain.Abstractions;
using Bonyan.TenantManagement.Domain;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface IRoleRepository : IRepository<TenantRoleEntity,Guid>
    {
        // Add any specific methods related to Role if needed
        List<TenantRoleEntity> GetByTenantId(TenantId tenantId);
        
    }
}
