using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface IRoleRepository : IRepository<TenantRoleEntity,Guid>
    {
        // Add any specific methods related to Role if needed
        List<TenantRoleEntity> GetByTenantId(Guid tenantId);
        
    }
}
