using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities;

public class Tenant : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Subdomain { get; private set; }
    public TenantStatus Status { get; private set; }

    // List of users associated with this tenant
    private List<TenantMember> _users = new List<TenantMember>();
    public ICollection<TenantMember> Users => _users;

    private List<TenantRole> _roles = new List<TenantRole>();
    public ICollection<TenantRole> Roles => _roles;
    
    private List<Invitation> _ivitations = new List<Invitation>();
    public ICollection<Invitation> Invitations => _ivitations;

    protected Tenant()
    {
    }

    public Tenant(string name, string subdomain)
    {
        Id = Guid.NewGuid(); // Tenant ID
        Name = name;
        Subdomain = subdomain;
        Status = TenantStatus.Active;
    }

    // Change the status of the tenant (e.g., Active, Suspended, Closed)
    public void ChangeStatus(TenantStatus newStatus)
    {
        if (Status == newStatus) return; // If no actual change, do nothing.
        Status = newStatus;
    }

    // Add a user to the tenant
    public void AddUser(TenantMember tenantMember)
    {
        if (!_users.Exists(u => u.UserId == tenantMember.UserId))
        {
            _users.Add(tenantMember);
        }
    }

    // Remove a user from the tenant
    public void RemoveUser(Guid userId)
    {
        var userTenant = _users.Find(u => u.UserId == userId);
        if (userTenant != null)
        {
            _users.Remove(userTenant);
        }
    }

    // Change the subdomain of the tenant (for renaming purposes)
    public void ChangeSubdomain(string newSubdomain)
    {
        if (!string.IsNullOrWhiteSpace(newSubdomain) && newSubdomain != Subdomain)
        {
            Subdomain = newSubdomain;
        }
    }
}

public enum TenantStatus
{
    Active,
    Suspended,
    Closed
}