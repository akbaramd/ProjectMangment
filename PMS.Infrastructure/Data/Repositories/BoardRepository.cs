using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;


namespace PMS.Infrastructure.Data.Repositories;

public class BoardRepository : EfCoreRepository< KanbanBoardEntity,Guid,ApplicationDbContext>, IBoardRepository
{
   

    public new Task<KanbanBoardEntity?> GetByIdAsync(Guid id)
    {
        return _dbContext.KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public List<KanbanBoardEntity> GetAllWithRelations()
    {
        return _dbContext.KanbanBoards
            .Include(b => b.Columns)
            .ToList();
    }

    public Task<KanbanBoardEntity?> GetByIdWithRelationsAsync(Guid boardId)
    {
        return _dbContext.KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public List<KanbanBoardEntity> GetByTenantId(Guid tenantId)
    {
        return _dbContext.KanbanBoards
            .Where(b => b.TenantId == tenantId)
            .Include(b => b.Columns)
            .ToList();
    }

    public List<KanbanBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId)
    {
        return _dbContext.KanbanBoards
            .Where(b => b.SprintId == sprintId)
            .Include(b => b.Columns)
            .ToList();
    }

    public BoardRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}