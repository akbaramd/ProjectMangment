using PMS.Domain.Entities;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectDetailsByIdForTenantSpec : Specification<Project>
{
    public ProjectDetailsByIdForTenantSpec(Guid id,Guid tenantId)
    {
        AddCriteria(x => x.TenantId == tenantId && x.Id == id);
        AddIncludeCollection(x => x.Members).ThenInclude(x => x.Member).ThenInclude(x => x.User);
        AddIncludeCollection(c => c.Sprints).ThenIncludeCollection(c=>c.Boards)
            .ThenIncludeCollection(c=>c.Columns)
            .ThenIncludeCollection(c=>c.Tasks);
        
    }
}
