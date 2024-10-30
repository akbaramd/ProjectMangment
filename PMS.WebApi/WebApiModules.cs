using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;
using Bonyan.TenantManagement.Web;
using PMS.Application;
using PMS.Infrastructure;

namespace PMS.WebApi;

[DependOn([
    typeof(BonyanTenantManagementWebModule),
    typeof(ApplicationModule),
    typeof(InfrastructureModule),
])]
public class WebApiModules : Module
{
    public override Task OnConfigureAsync(ServiceConfigurationContext context)
    {
        return base.OnConfigureAsync(context);
    }
}