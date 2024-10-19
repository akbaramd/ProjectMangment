using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories;

public interface IProjectRepository : IGenericRepository<Project>
{
    List<Project> GetAllWithRelations();
    Task<Project?> GetByIdWithRelationsAsync(Guid projectId);
    List<Project> GetByTenantId(Guid tenantId);
}