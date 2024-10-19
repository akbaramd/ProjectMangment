using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class BoardRepository : EfGenericRepository<ApplicationDbContext, Board>, IBoardRepository
{
    public BoardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<Board> GetAllWithRelations()
    {
        return _context.Boards
            .Include(b => b.Columns)
            .ToList();
    }

    public Task<Board?> GetByIdWithRelationsAsync(Guid boardId)
    {
        return _context.Boards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public List<Board> GetByTenantId(Guid tenantId)
    {
        return _context.Boards
            .Where(b => b.TenantId == tenantId)
            .Include(b => b.Columns)
            .ToList();
    }
}