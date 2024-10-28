namespace PMS.Application.UseCases.Tenants.Models;

public class TenantRoleCreateDto
{
    public string RoleName { get; set; }
    public List<string> PermissionKeys { get; set; }
}