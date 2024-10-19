namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IRoleSeeder
    {
        System.Threading.Tasks.Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames);
    }
}
