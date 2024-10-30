using Bonyan.Layer.Domain;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;


namespace PMS.Infrastructure.Data.Repositories;

public class BoardRepository : EfCoreRepository< KanbanBoardEntity,Guid,ApplicationDbContext>, IBoardRepository
{
   

    public new Task<KanbanBoardEntity?> GetByIdAsync(Guid id)
    {
        return  await (await GetDbContextAsync()).KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public List<KanbanBoardEntity> GetAllWithRelations()
    {
        return  await (await GetDbContextAsync()).KanbanBoards
            .Include(b => b.Columns)
            .ToList();
    }

    public Task<KanbanBoardEntity?> GetByIdWithRelationsAsync(Guid boardId)
    {
        return  await (await GetDbContextAsync()).KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public List<KanbanBoardEntity> GetByTenantId(Guid tenantId)
    {
        return  await (await GetDbContextAsync()).KanbanBoards
            .Where(b => b.TenantId == tenantId)
            .Include(b => b.Columns)
            .ToList();
    }

    public List<KanbanBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId)
    {
        return  await (await GetDbContextAsync()).KanbanBoards
            .Where(b => b.SprintId == sprintId)
            .Include(b => b.Columns)
            .ToList();
    }

    public BoardRepository(ApplicationDbContext dbContext,IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}