using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class RoleRepository : EfGenericRepository<ApplicationDbContext, ApplicationRole>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<ApplicationRole?> GetRoleByNameAsync(string roleName)
    {
        return _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }
}