using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;


namespace PMS.Infrastructure.Data.Repositories;

public class TaskRepository : EfGenericRepository<ApplicationDbContext, SprintTask>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<SprintTask> GetAllWithRelations()
    {
        return _context.SprintTasks
            .Include(t => t.Sprint)
            .Include(t => t.CurrentColumn)
            .ToList();
    }

    public Task<SprintTask?> GetByIdWithRelationsAsync(Guid taskId)
    {
        return _context.SprintTasks
            .Include(t => t.Sprint)
            .Include(t => t.CurrentColumn)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public List<SprintTask> GetBySprintId(Guid sprintId)
    {
        return _context.SprintTasks
            .Where(t => t.SprintId == sprintId)
            .Include(t => t.CurrentColumn)
            .ToList();
    }
}