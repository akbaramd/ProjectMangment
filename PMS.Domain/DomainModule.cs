using SharedKernel.Modularity;
using SharedKernel.Modularity.Abstractions;

namespace PMS.Domain;

public class DomainModule : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        return base.OnConfigureAsync(context);
    }
}