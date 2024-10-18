using PMS.Application.DTOs;
using PMS.Domain.Entities;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId);
    Task<AuthResponseDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto);
    Task<UserProfileDto> GetUserProfileAsync(Guid userId,string tenant);

    Task<List<RoleWithPermissionsDto>> GetRolesForTenantAsync(string tenantId);
    Task AddRoleAsync(string tenantId, CreateRoleDto createRoleDto);
    Task UpdateRoleAsync(string tenantId, Guid roleId, UpdateRoleDto updateRoleDto);
    Task DeleteRoleAsync(string tenantId, Guid roleId);
    Task<List<PermissionGroupDto>> GetPermissionGroupsAsync();
}