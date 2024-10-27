namespace PMS.Domain.BoundedContexts.TenantManagement
{
    public class TenantPermissionGroupEntity  
    {

        public string Key { get; set; }
        public string Name { get; set; }


        public virtual ICollection<TenantPermissionEntity> Permissions { get; set; } = new List<TenantPermissionEntity>();
        
        public TenantPermissionGroupEntity(){}
        
        public TenantPermissionGroupEntity(string key, string name)
        {
            Key = key;
            Name = name;
        }

    }
}
