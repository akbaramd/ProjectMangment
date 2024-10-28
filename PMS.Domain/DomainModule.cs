using Bonyan.DomainDrivenDesign.Domain;
using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;
using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;

namespace PMS.Domain;

[DependOn(typeof(BonyanDomainDrivenDesignDomainModule),typeof(BonyanTenantManagementDomainModule))]
public class DomainModule : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        return base.OnConfigureAsync(context);
    }
}