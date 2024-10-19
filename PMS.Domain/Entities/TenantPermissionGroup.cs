using System.Collections;
using Microsoft.AspNetCore.Identity;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    public class ApplicationPermissionGroup  
    {

        public string Key { get; set; }
        public string Name { get; set; }


        public ICollection<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
        
        public ApplicationPermissionGroup(){}
        
        public ApplicationPermissionGroup(string key, string name)
        {
            Key = key;
            Name = name;
        }

    }
}
