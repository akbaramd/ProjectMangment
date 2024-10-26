using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class BoardRepository : EfGenericRepository<ApplicationDbContext, KanbanBoardEntity>, IBoardRepository
{
    public BoardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public new Task<KanbanBoardEntity?> GetByIdAsync(Guid id)
    {
        return _context.KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public List<KanbanBoardEntity> GetAllWithRelations()
    {
        return _context.KanbanBoards
            .Include(b => b.Columns)
            .ToList();
    }

    public Task<KanbanBoardEntity?> GetByIdWithRelationsAsync(Guid boardId)
    {
        return _context.KanbanBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public List<KanbanBoardEntity> GetByTenantId(Guid tenantId)
    {
        return _context.KanbanBoards
            .Where(b => b.TenantId == tenantId)
            .Include(b => b.Columns)
            .ToList();
    }

    public List<KanbanBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId)
    {
        return _context.KanbanBoards
            .Where(b => b.SprintId == sprintId)
            .Include(b => b.Columns)
            .ToList();
    }
}