using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectDetailsByIdForTenantSpec : Specification<Project>
{
    public ProjectDetailsByIdForTenantSpec(Guid tenantId, Guid id)
    {
        TenantId = tenantId;
        Id = id;
    }

    public Guid TenantId { get; set; }
    public Guid Id { get; set; }

    public override void Handle(ISpecificationContext<Project> context)
    {
        context.AddCriteria(x => x.TenantId == TenantId && x.Id == Id);
        context.AddInclude(x => x.Members).ThenInclude(x => x.TenantMember).ThenInclude(x => x.User);
        context.AddInclude(c => c.Sprints).ThenInclude(c=>c.Boards)
            .ThenInclude(c=>c.Columns)
            .ThenInclude(c=>c.Tasks);
    }
}
