using Bonyan.AutoMapper;
using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;
using Bonyan.TenantManagement.Application;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.UseCases.Auth;
using PMS.Application.UseCases.Tenants;
using PMS.Application.UseCases.User;
using PMS.Domain;

namespace PMS.Application;

[DependOn(
    typeof(BonyanTenantManagementApplicationModule),
    typeof(DomainModule))]
public class ApplicationModule : Module
{
    public override Task OnConfigureAsync(ServiceConfigurationContext context)
    {
        // Add AutoMapper

        context.Configure<BonyanAutoMapperOptions>(c =>
        {
            c.AddProfile<TenantProfile>(true);
            // c.AddProfile<ProjectAndSprintProfile>(true);
            // c.AddProfile<InvitationProfile>(true);
            // c.AddProfile<SprintProfile>(true);
            // c.AddProfile<TaskProfile>(true);
            c.AddProfile<UserProfile>(true);
        });

        context.Services.AddScoped<IAuthService, AuthService>();
        context.Services.AddScoped<ITenantRoleService, TenantRoleService>();
        context.Services.AddScoped<ITenantService, TenantService>();
        // context.Services.AddScoped<IInvitationService, InvitationService>();
        // context.Services.AddScoped<ITaskService, TaskService>();
        // context.Services.AddScoped<IProjectService, ProjectService>();

        // Register application services
        // context.Services.AddScoped<IProjectService, ProjectService>();
        // context.Services.AddScoped<IBoardService, BoardService>();
        // context.Services.AddScoped<ISprintService, SprintService>();

        return base.OnConfigureAsync(context);
    }
}