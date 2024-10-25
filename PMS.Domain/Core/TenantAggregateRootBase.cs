using PMS.Domain.BoundedContexts.TenantManagment;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Core;

public abstract class TenantAggregateRootBase : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public virtual TenantEntity Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected  TenantAggregateRootBase(){}
    protected TenantAggregateRootBase(TenantEntity tenant)
    {
        if (tenant == null)
        {
            throw new ArgumentNullException(nameof(tenant), "Tenant cannot be null.");
        }

        TenantId = tenant.Id;
        Tenant = tenant;
        CreatedAt = DateTime.UtcNow;
    }

    // Method to change the associated tenantEntity
    public void ChangeTenant(TenantEntity newTenantEntity)
    {
        if (newTenantEntity == null)
        {
            throw new ArgumentNullException(nameof(newTenantEntity), "New tenantEntity cannot be null.");
        }

        Tenant = newTenantEntity;
        TenantId = newTenantEntity.Id;
        UpdatedAt = DateTime.UtcNow;
    }
}