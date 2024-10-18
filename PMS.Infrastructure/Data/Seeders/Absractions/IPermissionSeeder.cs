namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IPermissionSeeder
    {
        Task SeedAsync(string groupKey,string key,string title);
        Task SeedGroupAsync(string key,string title);
    }
}
