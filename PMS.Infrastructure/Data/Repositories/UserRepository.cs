using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

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
