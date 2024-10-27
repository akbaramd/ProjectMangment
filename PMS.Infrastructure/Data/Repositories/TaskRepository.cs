using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories;


namespace PMS.Infrastructure.Data.Repositories;

public class TaskRepository : EfCoreRepository< TaskEntity,Guid,ApplicationDbContext>, ITaskRepository
{
    public List<TaskEntity> GetAllWithRelations()
    {
        return _dbContext.Tasks
            .Include(t => t.BoardColumn)
            .ToList();
    }

    public Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId)
    {
        return _dbContext.Tasks
            .Include(t => t.BoardColumn)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public List<TaskEntity> GetBySprintId(Guid sprintId)
    {
        return _dbContext.Tasks
            .Include(t => t.BoardColumn)
            .ToList();
    }

    public Task<List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId)
    {
        return _dbContext.Tasks
            .Where(t => t.BoardColumnId == boardId)
            .Include(t => t.BoardColumn)
            .ToListAsync();
    }

    public TaskRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}