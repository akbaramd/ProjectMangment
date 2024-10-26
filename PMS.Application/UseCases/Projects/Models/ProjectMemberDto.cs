using PMS.Application.UseCases.Tenant.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;

namespace PMS.Application.UseCases.Projects.Models;

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public TenantMemberDto TenantMember { get; set; }
    public ProjectMemberAccessEnum AccessEnum { get; set; }
}