using Microsoft.AspNetCore.Identity;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    public class ApplicationPermission  
    {

        public string Key { get; set; }
        public string Name { get; set; }
        public string GroupKey { get; set; }
        
        public ICollection<ApplicationRole> Roles { get; set; }
        
        public ApplicationPermission(){}
        
        public ApplicationPermission(string groupKey ,string key, string name)
        {
            GroupKey = groupKey;
            Key = key;
            Name = name;
        }

    }
}
