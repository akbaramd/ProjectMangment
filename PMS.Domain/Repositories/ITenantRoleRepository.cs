using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface IRoleRepository : IGenericRepository<TenantRole>
    {
        // Add any specific methods related to Role if needed
        List<TenantRole> GetByTenantId(Guid tenantId);
        
    }
}
