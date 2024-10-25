using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class ProjectRepository : EfGenericRepository<ApplicationDbContext, ProjectEntity>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<ProjectEntity> GetAllWithRelations()
    {
        return _context.Projects
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .ToList();
    }

    public Task<ProjectEntity?> GetByIdWithRelationsAsync(Guid projectId)
    {
        return _context.Projects
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public List<ProjectEntity> GetByTenantId(Guid tenantId)
    {
        return _context.Projects
            .Where(p => p.TenantId == tenantId)
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .ToList();
    }
}