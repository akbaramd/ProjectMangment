using Bonyan.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace PMS.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlite("Data Source=../PMS.WebApi/pms.db");

        var services = new ServiceCollection();
        services.Configure<BonyanMultiTenancyOptions>(c =>
        {
            c.IsEnabled = true;
        });
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}