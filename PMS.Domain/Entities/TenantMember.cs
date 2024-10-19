using PMS.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Domain.Entities
{
    public class TenantMember : TenantEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; }
        public TenantMemberStatus MemberStatus { get; private set; }

        private readonly List<TenantRole> _roles = new List<TenantRole>();
        public IReadOnlyCollection<TenantRole> Roles => _roles.AsReadOnly(); // Expose as read-only collection

        protected TenantMember() { }

        public TenantMember(ApplicationUser user, Tenant tenant)
            : base(tenant)
        {
            UserId = user.Id;
            User = user;
            MemberStatus = TenantMemberStatus.Active;
        }

        // Change the status of the user within the tenant (Active, Inactive, Banned)
        public void ChangeStatus(TenantMemberStatus memberStatus)
        {
            if (MemberStatus == memberStatus)
            {
                throw new InvalidOperationException("The member already has the specified status.");
            }
            MemberStatus = memberStatus;
        }

        // Add a role to the member, if they don't already have it
        public void AddRole(TenantRole role)
        {
            if (HasRole(role))
            {
                throw new InvalidOperationException($"The member already has the role '{role.Key}'.");
            }
            _roles.Add(role);
        }

        // Remove a role from the member
        public void RemoveRole(TenantRole role)
        {
            if (!HasRole(role))
            {
                throw new InvalidOperationException($"The member does not have the role '{role.Key}' to remove.");
            }
            _roles.Remove(role);
        }

        // Check if the user has a specific role
        public bool HasRole(TenantRole role)
        {
            return _roles.Any(r => r.Id == role.Id);
        }

        // Clear all roles for the member
        public void ClearRoles()
        {
            _roles.Clear();
        }

        // Method to check if a specific permission key exists within the user's roles
        public bool HasPermission(string permissionKey)
        {
            return _roles.Any(role => role.Permissions.Any(permission => permission.Key == permissionKey));
        }
    }

    // Enum to represent the status of a TenantMember
    public enum TenantMemberStatus
    {
        Active,
        Inactive,
        Banned
    }
}
