using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public bool Deletable { get; private set; } = true;  // Default: roles can be deleted unless specified

        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName, bool deletable = true) : base()
        {
            Name = roleName;
            Deletable = deletable;
        }
    }
}
