using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface ITaskRepository : IGenericRepository<SprintTask>
    {
        List<SprintTask> GetAllWithRelations();
        Task<SprintTask?> GetByIdWithRelationsAsync(Guid taskId);
        List<SprintTask> GetBySprintId(Guid sprintId);
    }
}