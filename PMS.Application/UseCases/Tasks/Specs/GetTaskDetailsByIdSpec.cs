using PMS.Application.UseCases.Projects.Models;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TaskManagement;
using SharedKernel.Specification;

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
