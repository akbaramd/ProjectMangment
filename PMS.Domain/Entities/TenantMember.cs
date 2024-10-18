using PMS.Domain.Core;

namespace PMS.Domain.Entities;

public class TenantMember : TenantEntity
{
    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; }

    public TenantMemberStatus MemberStatus { get; private set; }
        
    // Change from ApplicationRole to UserTenantRole
    public TenantMemberRole MemberRole { get; private set; }

    protected TenantMember() { }

    public TenantMember(ApplicationUser user, Tenant tenant, TenantMemberRole memberRole)
        : base(tenant)
    {
        UserId = user.Id;
        User = user;
        MemberRole = memberRole;
        MemberStatus = TenantMemberStatus.Active;
    }

    // Change user status in tenant
    public void ChangeStatus(TenantMemberStatus memberStatus)
    {
        MemberStatus = memberStatus;
    }

    // Change the role of a user in the tenant
    public void ChangeRole(TenantMemberRole memberRole)
    {
        MemberRole = memberRole;
    }
}

public enum TenantMemberStatus
{
    Active,
    Inactive,
    Banned
}


public enum TenantMemberRole
{
    Owner,      // Full control over the tenant
    Manager,    // Manages projects and teams
    Employee,   // General staff member or worker
    // Optional roles if needed
    Administrator, // Manages system settings and users
    Guest          // Limited access for external collaborators
}