using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface IBoardColumnRepository : IGenericRepository<BoardColumn>
    {
        List<BoardColumn> GetAllWithRelations();
        Task<BoardColumn?> GetByIdWithRelationsAsync(Guid columnId);
        List<BoardColumn> GetByBoardId(Guid boardId);
    }
}