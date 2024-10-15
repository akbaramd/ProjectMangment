using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PMS.Infrastructure.Seeding;

namespace PMS.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, string connectionString)
        {
            // Application Layer Services
           
            
            // Domain Layer Services
            

            // Infrastructure Layer Services
            // Register your custom seeder services
        services.AddScoped<IRoleSeeder, RoleSeeder>();
        services.AddScoped<IUserSeeder, UserSeeder>();
        services.AddScoped<ITenantSeeder, TenantSeeder>();
        services.AddScoped<IUserTenantSeeder, UserTenantSeeder>();
        services.AddScoped<DatabaseSeeder>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=pms.db"));
            

            return services;
        }
    }
}
