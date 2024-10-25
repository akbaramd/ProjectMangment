using PMS.Domain.BoundedContexts.ProjectManagement;

namespace PMS.Application.DTOs;

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public TenantMemberDto TenantMember { get; set; }
    public ProjectMemberAccess Access { get; set; }
}