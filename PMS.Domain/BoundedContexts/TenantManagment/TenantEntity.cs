using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TenantManagment;

public class TenantEntity : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Subdomain { get; private set; }
    public TenantrStatus Status { get; private set; }

    // List of users associated with this tenantEntity
    private List<TenantMemberEntity> _users = new List<TenantMemberEntity>();
    public virtual ICollection<TenantMemberEntity> Users => _users;

    private List<TenantRoleEntity> _roles = new List<TenantRoleEntity>();
    public virtual ICollection<TenantRoleEntity> Roles => _roles;
    

    protected TenantEntity()
    {
    }

    public TenantEntity(string name, string subdomain)
    {
        Id = Guid.NewGuid(); // Tenant ID
        Name = name;
        Subdomain = subdomain;
        Status = TenantrStatus.Active;
    }

    // Change the status of the tenantEntity (e.g., Active, Suspended, Closed)
    public void ChangeStatus(TenantrStatus newStatus)
    {
        if (Status == newStatus) return; // If no actual change, do nothing.
        Status = newStatus;
    }

    // Add a user to the tenantEntity
    public void AddUser(TenantMemberEntity tenantMemberEntity)
    {
        if (!_users.Exists(u => u.UserId == tenantMemberEntity.UserId))
        {
            _users.Add(tenantMemberEntity);
        }
    }

    // Remove a user from the tenantEntity
    public void RemoveUser(Guid userId)
    {
        var userTenant = _users.Find(u => u.UserId == userId);
        if (userTenant != null)
        {
            _users.Remove(userTenant);
        }
    }

    // Change the subdomain of the tenantEntity (for renaming purposes)
    public void ChangeSubdomain(string newSubdomain)
    {
        if (!string.IsNullOrWhiteSpace(newSubdomain) && newSubdomain != Subdomain)
        {
            Subdomain = newSubdomain;
        }
    }
}

public class TenantrStatus : Enumeration
{
    public static TenantrStatus InActive = new TenantrStatus(0, nameof(InActive)); 
    public static TenantrStatus Active = new TenantrStatus(1, nameof(Active)); 
    public static TenantrStatus Banned = new TenantrStatus(2, nameof(Banned)); 
    public TenantrStatus(int id, string name) : base(id, name)
    {
    }
}