using PMS.Application;
using PMS.Domain;
using PMS.Infrastructure;
using SharedKernel.Modularity;
using SharedKernel.Modularity.Abstractions;
using SharedKernel.Modularity.Attributes;

namespace PMS.WebApi;

[DependOn(typeof(ApplicationModule), typeof(InfrastructureModule))]
public class WebApiModules : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        return base.OnConfigureAsync(context);
    }
}