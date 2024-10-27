using Bonyan.DomainDrivenDesign.Domain.Specifications;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectDetailsByIdForTenantSpec : Specification<ProjectEntity>
{
    public ProjectDetailsByIdForTenantSpec(Guid tenantId, Guid id)
    {
        TenantId = tenantId;
        Id = id;
    }

    public Guid TenantId { get; set; }
    public Guid Id { get; set; }

    public override void Handle(ISpecificationContext<ProjectEntity> context)
    {
        context.AddCriteria(x => x.TenantId == TenantId && x.Id == Id);
        context.AddInclude(x => x.Members).ThenInclude(x => x.TenantMember).ThenInclude(x => x.User);
    }
}
