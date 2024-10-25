using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
            

            return services;
        }
    }
}
