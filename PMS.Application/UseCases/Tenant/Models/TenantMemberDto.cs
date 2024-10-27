using PMS.Application.UseCases.User.Models;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Tenant.Models;

public class TenantMemberDto
{
    public Guid UserId { get; set; }
    public UserProfileDto User { get; set; }
    public List<TenantRoleDto> Roles { get; set; }
    public TenantMemberStatus Status { get; set; }
}