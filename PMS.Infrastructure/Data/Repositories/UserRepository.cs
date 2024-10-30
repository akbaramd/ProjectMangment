using Bonyan.Layer.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class UserRepository : EfCoreRepository< UserEntity,Guid,ApplicationDbContext>, IUserRepository{

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await (await GetDbContextAsync()).Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        var user =await  (await GetDbContextAsync()).Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

        return new[] { ""};
    }

    public Task<bool> CheckPasswordAsync(UserEntity userEntity, string password)
    {
        return Task.FromResult(userEntity.PasswordHash != null && userEntity.PasswordHash.Equals(password));
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await (await GetDbContextAsync()).Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }


    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}