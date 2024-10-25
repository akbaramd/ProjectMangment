using Microsoft.Extensions.DependencyInjection;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Domain;
using SharedKernel.Modularity;
using SharedKernel.Modularity.Abstractions;
using SharedKernel.Modularity.Attributes;

namespace PMS.Application;

[DependOn(typeof(DomainModule))]
public class ApplicationModule : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
        // Add AutoMapper
        context.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        context.Services.AddScoped<IAuthService, AuthService>();
        context.Services.AddScoped<ITenantRoleService, TenantRoleService>();
        context.Services.AddScoped<ITenantService, TenantService>();
        context.Services.AddScoped<IInvitationService, InvitationService>();
          
        context.Services.AddScoped<IProjectService, ProjectService>();
       
        // Register application services
        context.Services.AddScoped<IProjectService, ProjectService>();
        context.Services.AddScoped<IBoardService, BoardService>();
        context.Services.AddScoped<ISprintService, SprintService>();
        
        return base.OnConfigureAsync(context);
    }
}