using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.TenantMembers.Specs;

public class InvitationByTenantSpec : PaginatedAndSortableSpecification<Invitation>
{
    public Guid TenantId { get; }
    public InvitationFilterDto Dto { get; }

    public InvitationByTenantSpec(Guid tenantId, InvitationFilterDto dto) : base(dto.Skip,dto.Take,dto.SortBy,dto.SortDirection)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<Invitation> context)
    {
        context.AddInclude(x => x.Tenant);
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.PhoneNumber.Contains(Dto.Search));
        }
        
    }
}
