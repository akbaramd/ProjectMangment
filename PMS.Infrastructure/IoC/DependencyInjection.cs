using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Repositories;
using PMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Data.Repositories;
using PMS.Infrastructure.Data.Seeders;
using PMS.Infrastructure.Data.Seeders.Absractions;
using PMS.Infrastructure.Services;

namespace PMS.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string connectionString)
        {
            // Application Layer Services
            // (Add application layer services here if needed)
            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IInvitationService, InvitationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<ISmsService, FakeSmsService>();
            // Domain Layer Services
            // (Add domain layer services here if needed)

            // Infrastructure Layer Services
            // Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // Register repository services
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<ITenantMemberRepository, TenantMemberRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            // Register seeder services
            services.AddScoped<IPermissionSeeder, PermissionSeeder>();
            services.AddScoped<IRoleSeeder, RoleSeeder>();
            services.AddScoped<IUserSeeder, UserSeeder>();
            services.AddScoped<ITenantSeeder, TenantSeeder>();
            services.AddScoped<IUserTenantSeeder, UserTenantSeeder>();
            services.AddScoped<DatabaseSeeder>();

            // Configure database context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=pms.db"));

            return services;
        }
    }
}