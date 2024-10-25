using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Repositories
{
    public interface ISprintRepository : IGenericRepository<ProjectSprintEntity>
    {
        List<ProjectSprintEntity> GetAllWithRelations();
        Task<ProjectSprintEntity?> GetByIdWithRelationsAsync(Guid sprintId);
        List<ProjectSprintEntity> GetByProjectId(Guid projectId);
    }
}