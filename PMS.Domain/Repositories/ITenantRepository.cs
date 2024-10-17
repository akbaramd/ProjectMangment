using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        // Add any specific methods related to Tenant if needed
        Task<Tenant?> GetTenantBySubdomainAsync(string subdomain);
    }
}
