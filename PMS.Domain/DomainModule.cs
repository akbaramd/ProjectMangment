using Bonyan.DomainDrivenDesign.Domain;
using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;

namespace PMS.Domain;

[DependOn(typeof(BonyanDomainDrivenDesignDomainModule))]
public class DomainModule : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        return base.OnConfigureAsync(context);
    }
}