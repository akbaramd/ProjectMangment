using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

public interface IProjectRepository : IRepository<ProjectEntity,Guid>
{
    List<ProjectEntity> GetAllWithRelations();
    Task<ProjectEntity?> GetByIdWithRelationsAsync(Guid projectId);
    List<ProjectEntity> GetByTenantId(Guid tenantId);
    
    Task<ProjectMemberEntity?> GetMemberByTenantMemberIdAsync(Guid tenantMemberId);

    Task<ProjectMemberEntity?> GetMemberByIdAsync(Guid projectMemberId);
}