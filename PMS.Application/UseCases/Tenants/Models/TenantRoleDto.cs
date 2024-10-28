namespace PMS.Application.UseCases.Tenants.Models;

public class TenantRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public List<TenantPermissionDto> Permissions { get; set; }
    public bool Deletable { get; set; }
    public bool IsSystemRole { get; set; }
}