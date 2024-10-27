using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class SprintRepository : EfCoreRepository<ProjectSprintEntity,Guid,ApplicationDbContext>, ISprintRepository
{

    public List<ProjectSprintEntity> GetAllWithRelations()
    {
        return _dbContext.ProjectSprints
            .ToList();
    }

    public Task<ProjectSprintEntity?> GetByIdWithRelationsAsync(Guid sprintId)
    {
        return _dbContext.ProjectSprints
            .FirstOrDefaultAsync(s => s.Id == sprintId);
    }

    public List<ProjectSprintEntity> GetByProjectId(Guid projectId)
    {
        return _dbContext.ProjectSprints
            .Where(s => s.ProjectId == projectId)
            .ToList();
    }

    public SprintRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}