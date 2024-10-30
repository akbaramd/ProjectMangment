using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories
{
    public interface IBoardRepository : IRepository<KanbanBoardEntity,Guid>
    {
       
        List<KanbanBoardEntity> GetAllWithRelations();
        Task<KanbanBoardEntity?> GetByIdWithRelationsAsync(Guid boardId);
        List<KanbanBoardEntity> GetByTenantId(Guid tenantId);
        List<KanbanBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId);
    }
}