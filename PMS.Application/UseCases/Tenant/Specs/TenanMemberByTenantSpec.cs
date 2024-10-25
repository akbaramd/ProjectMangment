using PMS.Application.UseCases.Tenant.Models;
using PMS.Domain.BoundedContexts.TenantManagment;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Tenant.Specs;

public class TenanMemberByTenantSpec : PaginatedAndSortableSpecification<TenantMemberEntity>
{
    public Guid TenantId { get; }
    public TenantMembersFilterDto Dto { get; }

    public TenanMemberByTenantSpec(Guid tenantId, TenantMembersFilterDto dto) : base(dto.Skip,dto.Take,dto.SortBy,dto.SortDirection)
    {
        TenantId = tenantId;
        Dto = dto;
    }


    public override void Handle(ISpecificationContext<TenantMemberEntity> context)
    {
        context.AddInclude(x => x.User).AddInclude(x=>x.Roles).ThenInclude(x=>x.Permissions);
        context.AddCriteria(x => x.TenantId == TenantId);
        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.User.FullName.Contains(Dto.Search));
        }
        
    }
}
