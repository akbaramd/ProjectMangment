using PMS.Domain.Entities;
using SharedKernel.Domain;

namespace PMS.Domain.Core;

public abstract class TenantEntity : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected TenantEntity(Tenant tenant)
    {
        if (tenant == null)
        {
            throw new ArgumentNullException(nameof(tenant), "Tenant cannot be null.");
        }

        TenantId = tenant.Id;
        Tenant = tenant;
        CreatedAt = DateTime.UtcNow;
    }

    // Method to change the associated tenant
    public void ChangeTenant(Tenant newTenant)
    {
        if (newTenant == null)
        {
            throw new ArgumentNullException(nameof(newTenant), "New tenant cannot be null.");
        }

        Tenant = newTenant;
        TenantId = newTenant.Id;
        UpdatedAt = DateTime.UtcNow;
    }
}