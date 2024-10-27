using Microsoft.Extensions.DependencyInjection;

namespace PMS.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string connectionString)
        {
            

            return services;
        }
    }
}
