using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories
{
    public interface ITaskRepository : IRepository<TaskEntity,Guid>
    {
        List<TaskEntity> GetAllWithRelations();
        Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId);
        List<TaskEntity> GetBySprintId(Guid sprintId);
        Task< List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId);
    }
}