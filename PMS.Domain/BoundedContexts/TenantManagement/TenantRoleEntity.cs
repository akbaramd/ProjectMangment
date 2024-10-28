using Bonyan.TenantManagement.Domain.Bonyan.TenantManagement.Domain;
using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.TenantManagement
{
    public class TenantRoleEntity : TenantEntityBase
    {

        public string Title { get; set; }
        public string Key { get; set; }
        public bool Deletable { get; private set; } = true;  // Default: roles can be deleted unless specified

        public bool IsSystemRole { get;private  set; }


        public virtual ICollection<TenantPermissionEntity> Permissions { get; set; } = new List<TenantPermissionEntity>();
        
        
            
        private List<TenantMemberEntity> _members = new List<TenantMemberEntity>();
        public virtual ICollection<TenantMemberEntity> Members => _members;

        
        public TenantRoleEntity() : base() { }

        public TenantRoleEntity(string key ,TenantId tenantId, bool deletable = true,bool isSystemRole = true)
        {
            TenantId = tenantId.Value;
            Title = key;
            Key = key.ToLower().Replace(" ","-");
            IsSystemRole = isSystemRole;
            Deletable = deletable;
        
        }
    }
}
