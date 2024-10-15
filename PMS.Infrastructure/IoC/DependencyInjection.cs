using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PMS.Infrastructure.Seeding;
using PMS.Domain.Repositories;
using PMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace PMS.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string connectionString)
        {
            // Application Layer Services
            // (Add application layer services here if needed)

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
            services.AddScoped<IUserTenantRepository, UserTenantRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            
            // Register seeder services
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
