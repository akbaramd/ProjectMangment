using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface IBoardRepository : IGenericRepository<Board>
    {
       
        List<Board> GetAllWithRelations();
        Task<Board?> GetByIdWithRelationsAsync(Guid boardId);
        List<Board> GetByTenantId(Guid tenantId);
        List<Board> GetBoardsBySprintIdAsync(Guid sprintId);
    }
}