using PMS.Domain.Entities;

namespace PMS.Infrastructure.Seeding
{
    public interface IUserSeeder
    {
        Task<ApplicationUser> SeedUserAsync(string email, string fullName, string phoneNumber, string password);
    }
}
