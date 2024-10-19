using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class SprintRepository : EfGenericRepository<ApplicationDbContext, Sprint>, ISprintRepository
{
    public SprintRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<Sprint> GetAllWithRelations()
    {
        return _context.Sprints
            .Include(s => s.Tasks)
            .ToList();
    }

    public Task<Sprint?> GetByIdWithRelationsAsync(Guid sprintId)
    {
        return _context.Sprints
            .Include(s => s.Tasks)
            .FirstOrDefaultAsync(s => s.Id == sprintId);
    }

    public List<Sprint> GetByProjectId(Guid projectId)
    {
        return _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .Include(s => s.Tasks)
            .ToList();
    }
}