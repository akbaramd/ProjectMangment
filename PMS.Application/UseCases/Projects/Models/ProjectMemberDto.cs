using PMS.Application.UseCases.Tenants.Models;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;

namespace PMS.Application.UseCases.Projects.Models;

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public TenantMemberDto TenantMember { get; set; }
    public ProjectMemberAccessEnum AccessEnum { get; set; }
}