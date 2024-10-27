using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

public interface IProjectMemberRepository: IRepository<ProjectMemberEntity,Guid>
{
    
}