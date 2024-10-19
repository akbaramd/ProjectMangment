namespace PMS.Application.DTOs;

public class CreateRoleDto
{
    public string RoleName { get; set; }
    public List<string> PermissionKeys { get; set; }
}

public class UpdateRoleDto
{
    public string RoleName { get; set; }
    public List<string> PermissionKeys { get; set; }
}

public class RoleWithPermissionsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public List<PermissionDto> Permissions { get; set; }
    public bool Deletable { get; set; }
    public bool IsSystemRole { get; set; }
}

public class PermissionGroupDto
{
    public string Key { get; set; }
    public string Name { get; set; }
    public List<PermissionDto> Permissions { get; set; }
}

public class PermissionDto
{
    public string Key { get; set; }
    public string Name { get; set; }
}