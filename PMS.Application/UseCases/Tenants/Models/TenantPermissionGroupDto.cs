namespace PMS.Application.UseCases.Tenants.Models;

public class TenantPermissionGroupDto
{
    public string Key { get; set; }
    public string Name { get; set; }
    public List<TenantPermissionDto> Permissions { get; set; }
}