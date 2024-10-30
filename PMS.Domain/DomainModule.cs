using Bonyan.Layer.Domain;
using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;
using Bonyan.TenantManagement.Domain;

namespace PMS.Domain;

[DependOn(typeof(BonyanTenantManagementDomainModule))]
public class DomainModule : Module
{
    public override Task OnConfigureAsync(ServiceConfigurationContext context)
    {
        return base.OnConfigureAsync(context);
    }
}