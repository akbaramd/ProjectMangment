using System.Text;
using Bonyan.AspNetCore.Persistence.EntityFrameworkCore;
using Bonyan.Modularity;
using Bonyan.Modularity.Abstractions;
using Bonyan.Modularity.Attributes;
using Bonyan.MultiTenant;
using Bonyan.Persistence.EntityFrameworkCore.Sqlite;
using Bonyan.TenantManagement.EntityFrameworkCore.Bonyan.TenantManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Interfaces;
using PMS.Domain;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Data.Repositories;
using PMS.Infrastructure.Data.Seeders;
using PMS.Infrastructure.Data.Seeders.Absractions;
using PMS.Infrastructure.Services;

namespace PMS.Infrastructure;

[DependOn(
    typeof(BonyanPersistenceEntityFrameworkModule),
    typeof(BonyanTenantManagementEntityFrameworkModule),
    typeof(DomainModule))]
public class InfrastructureModule : Module
{
    public override Task OnPreConfigureAsync(ModularityContext context)
    {
        context.Configure<BonyanMultiTenancyOptions>(c =>
        {
            c.IsEnabled = true;
        });
        return base.OnPreConfigureAsync(context);
    }

    public override Task OnConfigureAsync(ModularityContext context)
    {
        // Register repository services
        context.Services.AddScoped<ITaskRepository, TaskRepository>();
        context.Services.AddScoped<IBoardRepository, BoardRepository>();
        context.Services.AddScoped<ISprintRepository, SprintRepository>();
        context.Services.AddScoped<IProjectRepository, ProjectRepository>();
        context.Services.AddScoped<IInvitationRepository, InvitationRepository>();
        context.Services.AddScoped<ITenantMemberRepository, TenantMemberRepository>();
        context.Services.AddScoped<IPermissionRepository, PermissionRepository>();
        context.Services.AddScoped<IRoleRepository, RoleRepository>();
        context.Services.AddScoped<IUserRepository, UserRepository>();
        context.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        // Register seeder services
        context.Services.AddScoped<IPermissionSeeder, PermissionSeeder>();
        context.Services.AddScoped<IRoleSeeder, RoleSeeder>();
        context.Services.AddScoped<IUserSeeder, UserSeeder>();
        context.Services.AddScoped<ITenantSeeder, TenantSeeder>();
        context.Services.AddScoped<IUserTenantSeeder, UserTenantSeeder>();
        context.Services.AddScoped<DatabaseSeeder>();

        context.Services.AddSingleton<IAttachmentFileService, AttachmentFileService>();
        context.Services.AddSingleton<ISmsService, FakeSmsService>();
        context.Services.AddScoped<IJwtService, JwtService>();
        // Configure database context
        context.Configure<EntityFrameworkDbContextOptions>(c =>
        {
            c.UseSqlite("Data Source=pms.db");
        });

        // Identity
        context.AddBonyanDbContext<ApplicationDbContext>(c => { c.AddDefaultRepositories(true); });

        // Configure JWT authentication
        var jwtSettings = context.Configuration.GetSection("JwtSettings");
        context.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
                };
            });

        context.Services.AddAuthorization();
        return base.OnConfigureAsync(context);
    }
}