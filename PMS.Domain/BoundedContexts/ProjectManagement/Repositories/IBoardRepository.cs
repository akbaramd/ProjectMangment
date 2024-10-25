using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Repositories
{
    public interface IBoardRepository : IGenericRepository<ProjectBoardEntity>
    {
       
        List<ProjectBoardEntity> GetAllWithRelations();
        Task<ProjectBoardEntity?> GetByIdWithRelationsAsync(Guid boardId);
        List<ProjectBoardEntity> GetByTenantId(Guid tenantId);
        List<ProjectBoardEntity> GetBoardsBySprintIdAsync(Guid sprintId);
    }
}