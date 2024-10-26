using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories
{
    public interface IBoardRepository : IGenericRepository<KanbanBoardEntity>
    {
       
        List<KanbanBoardEntity> GetAllWithRelations();
        Task<KanbanBoardEntity?> GetByIdWithRelationsAsync(Guid boardId);
        List<KanbanBoardEntity> GetByTenantId(Guid tenantId);
        List<KanbanBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId);
    }
}