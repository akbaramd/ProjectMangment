using Bonyan.Layer.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories;


namespace PMS.Infrastructure.Data.Repositories;

public class TaskRepository : EfCoreRepository< TaskEntity,Guid,ApplicationDbContext>, ITaskRepository
{
    public async Task<List<TaskEntity>> GetAllWithRelations()
    {
        return  await (await GetDbContextAsync()).Tasks
            .Include(t => t.BoardColumn)
            .ToListAsync();
    }

    public async Task<TaskEntity?> GetByIdWithRelationsAsync(Guid taskId)
    {
        return  await (await GetDbContextAsync()).Tasks
            .Include(t => t.BoardColumn)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async List<TaskEntity> GetBySprintId(Guid sprintId)
    {
        return  await (await GetDbContextAsync()).Tasks
            .Include(t => t.BoardColumn)
            .ToList();
    }

    public async Task<List<TaskEntity>> GetTasksByBoardIdAsync(Guid boardId)
    {
        return  await (await GetDbContextAsync()).Tasks
            .Where(t => t.BoardColumnId == boardId)
            .Include(t => t.BoardColumn)
            .ToListAsync();
    }

    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}