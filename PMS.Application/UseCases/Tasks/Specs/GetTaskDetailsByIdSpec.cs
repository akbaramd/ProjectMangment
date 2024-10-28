using Bonyan.DomainDrivenDesign.Domain.Specifications;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;

namespace PMS.Application.UseCases.Tasks.Specs;

public class GetTaskDetailsByIdSpec : Specification<TaskEntity>
{
    public GetTaskDetailsByIdSpec(Guid id)
    {
        Id = id;
    }

    public  Guid Id { get; set; }


    public override void Handle(ISpecificationContext<TaskEntity> context)
    {
        context.AddInclude(x => x.Comments).ThenInclude(x=>x.ProjectMember);
        context. AddCriteria(x => x.Id == Id);

    }
}
