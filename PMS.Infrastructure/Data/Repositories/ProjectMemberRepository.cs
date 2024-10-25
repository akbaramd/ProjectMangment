using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using SharedKernel.EntityFrameworkCore;

namespace PMS.Infrastructure.Data.Repositories;

public class ProjectMemberRepository : EfGenericRepository<ApplicationDbContext, ProjectMemberEntity>, IProjectMemberRepository
{
    public ProjectMemberRepository(ApplicationDbContext context) : base(context)
    {
    }

  
}