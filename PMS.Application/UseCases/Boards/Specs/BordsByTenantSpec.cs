using Bonyan.DomainDrivenDesign.Domain.Specifications;
using PMS.Application.UseCases.Boards.Models;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;

namespace PMS.Application.UseCases.Boards.Specs;

public class BordsByTenantSpec : PaginatedSpecification<KanbanBoardEntity>
{
    public Guid TenantId { get; }
    public BorderFilterDto Dto { get; }

    public BordsByTenantSpec(Guid tenantId, BorderFilterDto dto) : base(dto.Skip, dto.Take)
    {
        TenantId = tenantId;
        Dto = dto;
    }

    public override void Handle(ISpecificationContext<KanbanBoardEntity> context)
    {
        context.AddInclude(x => x.Columns);

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