using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories
{
    public interface ITaskRepository : IRepository<TaskEntity,Guid>
    {
        Task<List<TaskEntity>> GetAllWithRelations();
        Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId);
        List<TaskEntity> GetBySprintId(Guid sprintId);
        Task< List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId);
    }
}