using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class UserRepository : EfGenericRepository<ApplicationDbContext, ApplicationUser>, IUserRepository{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<ApplicationUser?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    
}