using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PMS.Infrastructure.Data;

namespace PMS.Infrastructure.IoC
{
    public static class ApplicationBuilderExtensions
    {
        public static async System.Threading.Tasks.Task UseCore(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<ApplicationBuilder>>();

                try
                {
                    // Get DbContext instance
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    // Apply pending migrations if necessary
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        logger.LogInformation("Applying migrations...");
                        await dbContext.Database.MigrateAsync();
                        logger.LogInformation("Migrations applied successfully.");
                    }

                    // Get DatabaseSeeder and run seeding process
                    // var databaseSeeder = services.GetRequiredService<DatabaseSeeder>();
                    logger.LogInformation("Starting database seeding...");
                    // await databaseSeeder.SeedDefaultUserAsync();
                    logger.LogInformation("Database seeding completed.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;  // Rethrow the exception to let the app fail to start if migrations or seeding fails
                }
            }
        }
    }
}
