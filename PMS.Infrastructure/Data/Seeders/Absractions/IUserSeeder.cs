using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Seeders.Absractions
{
    public interface IUserSeeder
    {
        Task<ApplicationUser> SeedUserAsync(string email, string fullName, string phoneNumber, string password);
    }
}
