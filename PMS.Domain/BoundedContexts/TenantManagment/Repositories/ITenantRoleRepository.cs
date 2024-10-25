using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TenantManagment.Repositories
{
    public interface IRoleRepository : IGenericRepository<TenantRoleEntity>
    {
        // Add any specific methods related to Role if needed
        List<TenantRoleEntity> GetByTenantId(Guid tenantId);
        
    }
}
