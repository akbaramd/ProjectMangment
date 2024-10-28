using Bonyan.DomainDrivenDesign.Domain;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class ProjectMemberRepository : EfCoreRepository<ProjectMemberEntity,Guid,ApplicationDbContext>, IProjectMemberRepository
{
    public ProjectMemberRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext,serviceProvider)
    {
    }
}