namespace PMS.Application.UseCases.Projects.Models;

public class ProjectAddMemberDto
{
    public Guid TenantMemberId { get; set; }
    public string Role { get; set; }
}