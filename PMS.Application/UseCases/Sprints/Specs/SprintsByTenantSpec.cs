using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Sprints.Specs;

public class SprintsByTenantSpec : PaginateSpecification<Sprint>
{
    public SprintsByTenantSpec(Guid tenantId, SprintFilterDto dto) : base(dto.Skip,dto.Take)
    {
        
        AddCriteria(x => x.TenantId == tenantId);
        if (dto.Search != null && !string.IsNullOrWhiteSpace(dto.Search))
        {
            AddCriteria(c => c.Name.Contains(dto.Search));
        }
        
        if (dto.ProjectId != null &&dto.ProjectId != Guid.Empty)
        {
            AddCriteria(c => c.ProjectId == dto.ProjectId);
        }
    }
}
