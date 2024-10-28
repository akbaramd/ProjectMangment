namespace PMS.Application.UseCases.Tenants.Models;

public class TenantRoleUpdateDto
{
    public string RoleName { get; set; }
    public List<string> PermissionKeys { get; set; }
}