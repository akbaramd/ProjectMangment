using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Sprints.Specs;

public class SprintsByTenantSpec : PaginatedSpecification<Sprint>
{
    public Guid TenantId { get; }
    public SprintFilterDto Dto { get; }

    public SprintsByTenantSpec(Guid tenantId, SprintFilterDto dto) : base(dto.Skip,dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<Sprint> context)
    {
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.Name.Contains(Dto.Search));
        }
        
        if (Dto.ProjectId != null &&Dto.ProjectId != Guid.Empty)
        {
            context.AddCriteria(c => c.ProjectId == Dto.ProjectId);
        }
    }
}
