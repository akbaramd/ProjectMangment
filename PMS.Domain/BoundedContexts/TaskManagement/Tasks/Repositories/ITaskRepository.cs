using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>
    {
        List<TaskEntity> GetAllWithRelations();
        Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId);
        List<TaskEntity> GetBySprintId(Guid sprintId);
        Task< List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId);
    }
}