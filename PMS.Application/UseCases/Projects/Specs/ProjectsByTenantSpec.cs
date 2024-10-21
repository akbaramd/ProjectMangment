using PMS.Domain.Entities;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectsByTenantSpec : Specification<Project>
{
    public ProjectsByTenantSpec(Guid tenantId)
    {
        AddCriteria(x => x.TenantId == tenantId);
        
        AddIncludeCollection(c => c.Sprints).ThenIncludeCollection(c=>c.Boards)
            .ThenIncludeCollection(c=>c.Columns)
            .ThenIncludeCollection(c=>c.Tasks);
        
    }
}
