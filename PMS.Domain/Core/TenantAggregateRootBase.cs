using Bonyan.DomainDrivenDesign.Domain.Aggregates;
using Bonyan.MultiTenant;


namespace PMS.Domain.Core;

public abstract class TenantAggregateRootBase : AggregateRoot<Guid>,IMultiTenant
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected  TenantAggregateRootBase(){}

    // Method to change the associated tenantEntity
    public void ChangeTenant(Guid newTenantEntity)
    {
        if (newTenantEntity == null)
        {
            throw new ArgumentNullException(nameof(newTenantEntity), "New tenantEntity cannot be null.");
        }

        TenantId = newTenantEntity;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid? TenantId { get; private set; }
}