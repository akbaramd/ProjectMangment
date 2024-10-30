using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories
{
    public interface ISprintRepository : IRepository<ProjectSprintEntity,Guid>
    {
        List<ProjectSprintEntity> GetAllWithRelations();
        Task<ProjectSprintEntity?> GetByIdWithRelationsAsync(Guid sprintId);
        List<ProjectSprintEntity> GetByProjectId(Guid projectId);
    }
}