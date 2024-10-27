using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class UserRepository : EfCoreRepository< ApplicationUser,Guid,ApplicationDbContext>, IUserRepository{

    public Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<ApplicationUser?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }


    public UserRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}