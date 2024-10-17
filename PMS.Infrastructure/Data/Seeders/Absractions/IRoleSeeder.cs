namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IRoleSeeder
    {
        Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames);
    }
}
