using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Repositories;

public interface IProjectRepository : IGenericRepository<ProjectEntity>
{
    List<ProjectEntity> GetAllWithRelations();
    Task<ProjectEntity?> GetByIdWithRelationsAsync(Guid projectId);
    List<ProjectEntity> GetByTenantId(Guid tenantId);
}