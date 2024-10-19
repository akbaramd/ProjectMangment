using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class BoardColumnRepository : EfGenericRepository<ApplicationDbContext, BoardColumn>, IBoardColumnRepository
{
    public BoardColumnRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<BoardColumn> GetAllWithRelations()
    {
        return _context.BoardColumns
            .Include(bc => bc.Tasks)
            .ToList();
    }

    public Task<BoardColumn?> GetByIdWithRelationsAsync(Guid columnId)
    {
        return _context.BoardColumns
            .Include(bc => bc.Tasks)
            .FirstOrDefaultAsync(bc => bc.Id == columnId);
    }

    public List<BoardColumn> GetByBoardId(Guid boardId)
    {
        return _context.BoardColumns
            .Where(bc => bc.BoardId == boardId)
            .Include(bc => bc.Tasks)
            .ToList();
    }
}