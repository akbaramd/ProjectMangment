namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IPermissionSeeder
    {
        System.Threading.Tasks.Task SeedAsync(string groupKey,string key,string title);
        System.Threading.Tasks.Task SeedGroupAsync(string key,string title);
        Task<PermissionsData> SeedFromJsonAsync(string jsonFilePath);
    }
}
