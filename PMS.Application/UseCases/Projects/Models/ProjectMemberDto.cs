using PMS.Application.UseCases.Tenant.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;

namespace PMS.Application.UseCases.Projects.Models;

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public TenantMemberDto TenantMember { get; set; }
    public ProjectMemberAccess Access { get; set; }
}