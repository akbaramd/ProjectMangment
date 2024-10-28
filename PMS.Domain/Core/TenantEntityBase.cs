using Bonyan.DomainDrivenDesign.Domain.Entities;
using Bonyan.MultiTenant;
using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;

namespace PMS.Domain.Core;

public abstract class TenantEntityBase : Entity<Guid>,IMultiTenant
{
    public Guid? TenantId { get; protected set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected  TenantEntityBase(){}


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
}