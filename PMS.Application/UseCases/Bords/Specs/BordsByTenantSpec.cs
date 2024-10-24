using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Bords.Specs;

public class BordsByTenantSpec : PaginatedSpecification<Board>
{
    public Guid TenantId { get; }
    public BorderFilterDto Dto { get; }

    public BordsByTenantSpec(Guid tenantId, BorderFilterDto dto) : base(dto.Skip, dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }

    public override void Handle(ISpecificationContext<Board> context)
    {
        context.AddInclude(x => x.Columns).ThenInclude(x => x.Tasks);

        context.AddCriteria(x => x.TenantId == TenantId);

        if (Dto.Search != null && !string.IsNullOrWhiteSpace(Dto.Search))
        {
            context.AddCriteria(c => c.Name.Contains(Dto.Search));
        }

        if (Dto.SprintId != null && Dto.SprintId != Guid.Empty)
        {
            context.AddCriteria(c => c.SprintId == Dto.SprintId);
        }
    }
}