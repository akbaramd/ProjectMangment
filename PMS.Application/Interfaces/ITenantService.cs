using PMS.Application.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Interfaces
{
    public interface ITenantService
    {
        // Get tenant info along with members (only Owner, Manager, Administrator can view members)
        Task<TenantDto> GetTenantInfoAsync(string tenantId, Guid userId);

        // Remove a member from the tenant (only Owner, Manager, Administrator can perform this action)
        Task RemoveMemberAsync(string tenantId, Guid userId, Guid memberToRemoveId);

        // Update a member's role in the tenant (only Owner, Manager, Administrator can perform this action)
        Task UpdateMemberRoleAsync(string tenantId, Guid userId, Guid memberToUpdateId, TenantMemberRole newRole);
    }
}
