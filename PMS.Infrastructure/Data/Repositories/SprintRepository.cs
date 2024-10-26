using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class SprintRepository : EfGenericRepository<ApplicationDbContext, ProjectSprintEntity>, ISprintRepository
{
    public SprintRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<ProjectSprintEntity> GetAllWithRelations()
    {
        return _context.ProjectSprints
            .ToList();
    }

    public Task<ProjectSprintEntity?> GetByIdWithRelationsAsync(Guid sprintId)
    {
        return _context.ProjectSprints
            .FirstOrDefaultAsync(s => s.Id == sprintId);
    }

    public List<ProjectSprintEntity> GetByProjectId(Guid projectId)
    {
        return _context.ProjectSprints
            .Where(s => s.ProjectId == projectId)
            .ToList();
    }
}