using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class BoardColumnRepository : EfGenericRepository<ApplicationDbContext, ProjectBoardColumnEntity>, IBoardColumnRepository
{
    public BoardColumnRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<ProjectBoardColumnEntity> GetAllWithRelations()
    {
        return _context.ProjectBoardColumns
            .Include(bc => bc.Tasks)
            .ToList();
    }

    public Task<ProjectBoardColumnEntity?> GetByIdWithRelationsAsync(Guid columnId)
    {
        return _context.ProjectBoardColumns
            .Include(bc => bc.Tasks)
            .FirstOrDefaultAsync(bc => bc.Id == columnId);
    }

    public List<ProjectBoardColumnEntity> GetByBoardId(Guid boardId)
    {
        return _context.ProjectBoardColumns
            .Where(bc => bc.BoardId == boardId)
            .Include(bc => bc.Tasks)
            .ToList();
    }
}