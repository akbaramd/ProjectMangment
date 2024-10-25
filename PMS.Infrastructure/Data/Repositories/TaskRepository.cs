using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.TaskManagment.Repositories;
using SharedKernel.EntityFrameworkCore;


namespace PMS.Infrastructure.Data.Repositories;

public class TaskRepository : EfGenericRepository<ApplicationDbContext, TaskEntity>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<TaskEntity> GetAllWithRelations()
    {
        return _context.Tasks
            .Include(t => t.BoardColumn)
            .ThenInclude(t => t.Board)
            .ThenInclude(t => t.Sprint)
            .ThenInclude(t => t.Project)
            .ToList();
    }

    public Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId)
    {
        return _context.Tasks
            .Include(t => t.BoardColumn)
            .ThenInclude(t => t.Board)
            .ThenInclude(t => t.Sprint)
            .ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public List<TaskEntity> GetBySprintId(Guid sprintId)
    {
        return _context.Tasks
            .Include(t => t.BoardColumn)
            .ThenInclude(t => t.Board)
            .ThenInclude(t => t.Sprint)
            .ThenInclude(t => t.Project)
            .ToList();
    }

    public Task<List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId)
    {
        return _context.Tasks
            .Where(t => t.BoardColumnId == boardId)
            .Include(t => t.BoardColumn)
            .ThenInclude(t => t.Board)
            .ThenInclude(t => t.Sprint)
            .ThenInclude(t => t.Project)
            .ToListAsync();
    }
}