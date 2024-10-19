using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using PMS.Infrastructure.Data.Seeders.Absractions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserSeeder : IUserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> SeedUserAsync(string email, string fullName, string phoneNumber, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser(fullName, phoneNumber, email, deletable: false);
                user.GenerateRefreshToken();
                var result = await _userManager.CreateAsync(user, password);
            }
            return user;
        }
    }
}
