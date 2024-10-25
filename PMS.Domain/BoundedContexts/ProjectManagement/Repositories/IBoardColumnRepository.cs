using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Repositories
{
    public interface IBoardColumnRepository : IGenericRepository<ProjectBoardColumnEntity>
    {
        List<ProjectBoardColumnEntity> GetAllWithRelations();
        Task<ProjectBoardColumnEntity?> GetByIdWithRelationsAsync(Guid columnId);
        List<ProjectBoardColumnEntity> GetByBoardId(Guid boardId);
    }
}