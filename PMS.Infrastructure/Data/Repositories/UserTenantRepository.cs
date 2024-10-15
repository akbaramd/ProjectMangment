using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

public class UserTenantRepository : EfGenericRepository<ApplicationDbContext, UserTenant>, IUserTenantRepository
{
    public UserTenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<UserTenant?> GetUserTenantByUserIdAndTenantIdAsync(Guid userId, Guid tenantId)
    {
        return _context.UserTenants.FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
    }

    public Task<List<UserTenant>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        return _context.UserTenants.Where(ut => ut.TenantId == tenantId).ToListAsync();
    }

    public Task<List<UserTenant>> GetTenantsByUserIdAsync(Guid userId)
    {
        return _context.UserTenants.Where(ut => ut.UserId == userId).ToListAsync();
    }

    public Task<bool> IsUserInTenantAsync(Guid id, Guid tenantId)
    {
        return _context.UserTenants.AnyAsync(ut => ut.UserId == id && ut.TenantId == tenantId);
    }
}
