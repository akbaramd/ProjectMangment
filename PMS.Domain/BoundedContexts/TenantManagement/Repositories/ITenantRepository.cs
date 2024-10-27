using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TenantManagement.Repositories
{
    public interface ITenantRepository : IRepository<TenantEntity,Guid>
    {
        // Add any specific methods related to Tenant if needed
        Task<TenantEntity?> GetTenantBySubdomainAsync(string subdomain);
    }
}
