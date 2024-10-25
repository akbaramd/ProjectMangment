namespace PMS.Application.DTOs;

public class ProjectAddMemberDto
{
    public Guid TenantMemberId { get; set; }
    public string Role { get; set; }
}