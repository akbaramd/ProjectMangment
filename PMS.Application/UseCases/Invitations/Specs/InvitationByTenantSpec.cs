using Bonyan.DomainDrivenDesign.Domain.Specifications;
using PMS.Application.UseCases.Invitations.Models;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Invitations.Specs;

public class InvitationByTenantSpec : PaginatedAndSortableSpecification<ProjectInvitationEntity>
{
    public Guid TenantId { get; }
    public InvitationFilterDto Dto { get; }

    public InvitationByTenantSpec(Guid tenantId, InvitationFilterDto dto) : base(dto.Skip,dto.Take,dto.SortBy,dto.SortDirection)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<ProjectInvitationEntity> context)
    {
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.PhoneNumber.Contains(Dto.Search));
        }
        
    }
}
