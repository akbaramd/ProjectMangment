namespace PMS.Domain.BoundedContexts.TenantManagment
{
    public class TenantPermissionEntity  
    {

        public string Key { get; set; }
        public string Name { get; set; }
        public string GroupKey { get; set; }
        
        public virtual ICollection<TenantRoleEntity> Roles { get; set; }
        public virtual TenantPermissionGroupEntity Group { get; set; }
        
        public TenantPermissionEntity(){}
        
        public TenantPermissionEntity(string groupKey ,string key, string name)
        {
            GroupKey = groupKey;
            Key = key;
            Name = name;
        }

    }
}
