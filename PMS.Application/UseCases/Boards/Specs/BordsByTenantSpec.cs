using PMS.Application.UseCases.Boards.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Boards.Specs;

public class BordsByTenantSpec : PaginatedSpecification<ProjectBoardEntity>
{
    public Guid TenantId { get; }
    public BorderFilterDto Dto { get; }

    public BordsByTenantSpec(Guid tenantId, BorderFilterDto dto) : base(dto.Skip, dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }

    public override void Handle(ISpecificationContext<ProjectBoardEntity> context)
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