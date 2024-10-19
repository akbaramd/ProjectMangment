using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class ProjectRepository : EfGenericRepository<ApplicationDbContext, Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<Project> GetAllWithRelations()
    {
        return _context.Projects
            .Include(p => p.Sprints)
            .ThenInclude(p => p.Boards)
            .ThenInclude(p => p.Columns)
            .ToList();
    }

    public Task<Project?> GetByIdWithRelationsAsync(Guid projectId)
    {
        return _context.Projects
            .Include(p => p.Sprints)
            .ThenInclude(p => p.Boards)
            .ThenInclude(p => p.Columns)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public List<Project> GetByTenantId(Guid tenantId)
    {
        return _context.Projects
            .Where(p => p.TenantId == tenantId)
            .Include(p => p.Sprints)
            .ThenInclude(p => p.Boards)
            .ThenInclude(p => p.Columns)
            .ToList();
    }
}