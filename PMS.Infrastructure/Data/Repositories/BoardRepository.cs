using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class BoardRepository : EfGenericRepository<ApplicationDbContext, ProjectBoardEntity>, IBoardRepository
{
    public BoardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public new Task<ProjectBoardEntity?> GetByIdAsync(Guid id)
    {
        return _context.ProjectBoards
            .Include(b => b.Columns)
            .ThenInclude(b => b.Tasks)
            .FirstOrDefaultAsync(x=>x.Id == id);
    }

    public List<ProjectBoardEntity> GetAllWithRelations()
    {
        return _context.ProjectBoards
            .Include(b => b.Columns)
            .ToList();
    }

    public Task<ProjectBoardEntity?> GetByIdWithRelationsAsync(Guid boardId)
    {
        return _context.ProjectBoards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public List<ProjectBoardEntity> GetByTenantId(Guid tenantId)
    {
        return _context.ProjectBoards
            .Where(b => b.TenantId == tenantId)
            .Include(b => b.Columns)
            .ToList();
    }

    public List<ProjectBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId)
    {
        return _context.ProjectBoards
            .Where(b => b.SprintId == sprintId)
            .Include(b => b.Columns)
            .ToList();
    }
}