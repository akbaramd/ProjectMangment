using Bonyan.DomainDrivenDesign.Domain;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;

namespace PMS.Infrastructure.Data.Repositories;

public class ProjectRepository : EfCoreRepository< ProjectEntity,Guid,ApplicationDbContext>, IProjectRepository
{

    public List<ProjectEntity> GetAllWithRelations()
    {
        return _dbContext.Projects
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .ToList();
    }

    public Task<ProjectEntity?> GetByIdWithRelationsAsync(Guid projectId)
    {
        return _dbContext.Projects
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public List<ProjectEntity> GetByTenantId(Guid tenantId)
    {
        return _dbContext.Projects
            .Where(p => p.TenantId == tenantId)
            .Include(x=>x.Members)
            .ThenInclude(p => p.TenantMember)
            .ThenInclude(p => p.User)
            .ToList();
    }

    public async Task<ProjectMemberEntity?> GetMemberByTenantMemberIdAsync(Guid tenantMemberId)
    {
        return await _dbContext.ProjectsMembers
            .Include(x => x.TenantMember)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(p => p.TenantMemberId == tenantMemberId);
    }

    public async Task<ProjectMemberEntity?> GetMemberByIdAsync(Guid projectMemberId)
    {
        return await _dbContext.ProjectsMembers
            .Include(x => x.TenantMember)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectMemberId);
    }

    public ProjectRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
    {
    }
}