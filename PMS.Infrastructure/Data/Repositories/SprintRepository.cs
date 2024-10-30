using Bonyan.Layer.Domain;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class SprintRepository : EfCoreRepository<ProjectSprintEntity,Guid,ApplicationDbContext>, ISprintRepository
{

    public List<ProjectSprintEntity> GetAllWithRelations()
    {
        return  await (await GetDbContextAsync()).ProjectSprints
            .ToList();
    }

    public Task<ProjectSprintEntity?> GetByIdWithRelationsAsync(Guid sprintId)
    {
        return  await (await GetDbContextAsync()).ProjectSprints
            .FirstOrDefaultAsync(s => s.Id == sprintId);
    }

    public List<ProjectSprintEntity> GetByProjectId(Guid projectId)
    {
        return  await (await GetDbContextAsync()).ProjectSprints
            .Where(s => s.ProjectId == projectId)
            .ToList();
    }

    public SprintRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}