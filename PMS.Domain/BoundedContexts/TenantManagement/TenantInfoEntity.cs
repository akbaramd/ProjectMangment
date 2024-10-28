using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.TenantManagement
{
    
    public class TenantInfoEntity : TenantEntityBase
    {
        public TenantInfoEntity(string name)
        {
            Name = name;
        }
        protected TenantInfoEntity(){}

        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
