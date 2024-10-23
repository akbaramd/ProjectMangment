using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectsByTenantSpec : PaginatedSpecification<Project>
{
    public Guid TenantId { get; }
    public ProjectFilterDto Dto { get; }

    public ProjectsByTenantSpec(Guid tenantId, ProjectFilterDto dto) : base(dto.Skip,dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<Project> context)
    {
        context.AddInclude(x => x.Members).ThenInclude(x => x.TenantMember).ThenInclude(x => x.User);
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.Name.Contains(Dto.Search));
        }
    }
}
