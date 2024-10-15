using PMS.Domain.Entities;
using SharedKernel.Domain;

namespace PMS.Domain.Repositories
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        // Add any specific methods related to Tenant if needed
        Task<Tenant?> GetTenantBySubdomainAsync(string subdomain);
    }
}
