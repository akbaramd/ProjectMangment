using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {

        public string Title { get; set; }
        public bool Deletable { get; private set; } = true;  // Default: roles can be deleted unless specified

        public bool IsSystemRole { get;private  set; }

        public Guid? TenantId { get;private set; }
        public Tenant? Tenant { get;private  set; }
        public ICollection<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
        
        public ApplicationRole() : base() { }

        public ApplicationRole(string key , Guid? tenantId = null, bool deletable = true,bool isSystemRole = true) : base(tenantId == null?key:$"{key}:{tenantId}")
        {
            Title = key;
            IsSystemRole = isSystemRole;
            Deletable = deletable;
            TenantId = tenantId;
        }
    }
}
