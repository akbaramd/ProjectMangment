using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Interfaces;
using PMS.Domain;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Data.Repositories;
using PMS.Infrastructure.Data.Seeders;
using PMS.Infrastructure.Data.Seeders.Absractions;
using PMS.Infrastructure.Services;
using SharedKernel.Modularity;
using SharedKernel.Modularity.Abstractions;
using SharedKernel.Modularity.Attributes;

namespace PMS.Infrastructure;
[DependOn(typeof(DomainModule))]
public class InfrastructureModule : Module
{
    public override Task OnConfigureAsync(ModularityContext context)
    {
      
         
            // Register repository services
            context.Services.AddScoped<ITaskRepository, TaskRepository>();
            context.Services.AddScoped<IBoardRepository, BoardRepository>();
            context.Services.AddScoped<ISprintRepository, SprintRepository>();
            context.Services.AddScoped<IProjectRepository, ProjectRepository>();
            context.Services.AddScoped<IInvitationRepository, InvitationRepository>();
            context.Services.AddScoped<ITenantMemberRepository, TenantMemberRepository>();
            context.Services.AddScoped<ITenantRepository, TenantRepository>();
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
            context.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=pms.db"));
            
            // Identity
            context.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
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