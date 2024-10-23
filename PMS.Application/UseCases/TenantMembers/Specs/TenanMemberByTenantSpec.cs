using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.TenantMembers.Specs;

public class TenanMemberByTenantSpec : PaginatedSpecification<TenantMember>
{
    public Guid TenantId { get; }
    public TenantMembersFilterDto Dto { get; }

    public TenanMemberByTenantSpec(Guid tenantId, TenantMembersFilterDto dto) : base(dto.Skip,dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<TenantMember> context)
    {
        context.AddInclude(x => x.User).AddInclude(x=>x.Roles).ThenInclude(x=>x.Permissions);
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.User.FullName.Contains(Dto.Search));
        }
        
    }
}
