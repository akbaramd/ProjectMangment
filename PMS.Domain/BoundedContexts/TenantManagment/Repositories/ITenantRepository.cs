using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TenantManagment.Repositories
{
    public interface ITenantRepository : IGenericRepository<TenantEntity>
    {
        // Add any specific methods related to Tenant if needed
        Task<TenantEntity?> GetTenantBySubdomainAsync(string subdomain);
    }
}
