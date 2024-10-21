using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectsByTenantSpec : PaginateSpecification<Project>
{
    public ProjectsByTenantSpec(Guid tenantId, ProjectFilterDto dto) : base(dto.Skip,dto.Take)
    {
        AddCriteria(x => x.TenantId == tenantId);
        if (dto.Search != null && !string.IsNullOrWhiteSpace(dto.Search))
        {
            AddCriteria(c => c.Name.Contains(dto.Search));
        }
    }
}
