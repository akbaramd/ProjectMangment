using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

public interface IProjectRepository : IGenericRepository<ProjectEntity>
{
    List<ProjectEntity> GetAllWithRelations();
    Task<ProjectEntity?> GetByIdWithRelationsAsync(Guid projectId);
    List<ProjectEntity> GetByTenantId(Guid tenantId);
    
    Task<ProjectMemberEntity?> GetMemberByTenantMemberIdAsync(Guid tenantMemberId);

    Task<ProjectMemberEntity?> GetMemberByIdAsync(Guid projectMemberId);
}