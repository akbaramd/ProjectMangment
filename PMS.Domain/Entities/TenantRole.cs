using Microsoft.AspNetCore.Identity;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    public class TenantRole : Entity<Guid>
    {

        public string Title { get; set; }
        public string Key { get; set; }
        public bool Deletable { get; private set; } = true;  // Default: roles can be deleted unless specified

        public bool IsSystemRole { get;private  set; }

        public Guid? TenantId { get;private set; }
        public Tenant? Tenant { get;private  set; }
        public ICollection<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
        
        
            
        private List<TenantMember> _members = new List<TenantMember>();
        public ICollection<TenantMember> Members => _members;

        
        public TenantRole() : base() { }

        public TenantRole(string key , Guid? tenantId = null, bool deletable = true,bool isSystemRole = true) 
        {
            Title = key;
            Key = key.ToLower().Replace(" ","-");
            IsSystemRole = isSystemRole;
            Deletable = deletable;
            TenantId = tenantId;
        }
    }
}
