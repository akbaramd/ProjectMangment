namespace PMS.Infrastructure.Seeding
{
    public interface IRoleSeeder
    {
        Task SeedRoleAsync(string roleName, IEnumerable<string> policyNames);
    }
}
