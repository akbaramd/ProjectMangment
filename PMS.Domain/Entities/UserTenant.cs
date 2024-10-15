using PMS.Domain.Core;

namespace PMS.Domain.Entities
{
    public class UserTenant : TenantEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; }

        public UserTenantStatus Status { get; private set; }
        
        // Change from ApplicationRole to UserTenantRole
        public UserTenantRole Role { get; private set; }

        protected UserTenant() { }

        public UserTenant(ApplicationUser user, Tenant tenant, UserTenantRole role)
            : base(tenant)
        {
            UserId = user.Id;
            User = user;
            Role = role;
            Status = UserTenantStatus.Active;
        }

        // Change user status in tenant
        public void ChangeStatus(UserTenantStatus status)
        {
            Status = status;
        }

        // Change the role of a user in the tenant
        public void ChangeRole(UserTenantRole role)
        {
            Role = role;
        }
    }

    public enum UserTenantStatus
    {
        Active,
        Inactive,
        Banned
    }
}


public enum UserTenantRole
{
    Owner,     // The owner of the tenant (company)
    Manager,   // Manages the company
    Maintainer, // Maintains the resources, manages operational aspects
    Employer   // General employee or worker
}