using Bonyan.Modularity;
using Bonyan.Modularity.Attributes;
using PMS.Application;
using PMS.Infrastructure;

namespace PMS.WebApi;

[DependOn(typeof(ApplicationModule), typeof(InfrastructureModule))]
public class WebApiModules : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        return base.OnConfigureAsync(context);
    }
}