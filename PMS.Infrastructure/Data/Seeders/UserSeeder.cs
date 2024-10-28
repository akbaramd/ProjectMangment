using Microsoft.AspNetCore.Identity;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using PMS.Infrastructure.Data.Seeders.Absractions;

namespace PMS.Infrastructure.Data.Seeders
{
    public class UserSeeder : IUserSeeder
    {
        private readonly IUserRepository _userManager;

        public UserSeeder(IUserRepository userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> SeedUserAsync(string email, string fullName, string phoneNumber, string password)
        {
            var user = await _userManager.FindOneAsync(x=>x.Email.Equals(email));
            if (user == null)
            {
                user = new ApplicationUser(fullName, phoneNumber, email, deletable: false);
                user.GenerateRefreshToken();
                user.PasswordHash = password;
                var result = await _userManager.AddAsync(user);
            }
            return user;
        }
    }
}
