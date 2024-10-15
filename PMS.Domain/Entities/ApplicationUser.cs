using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace PMS.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; private set; }
        public bool Deletable { get; private set; } = true;  // Default: users can be deleted unless specified
        public bool IsPhoneNumberConfirmed
        {
            get => PhoneNumberConfirmed;
            set => PhoneNumberConfirmed = value;
        }

        public UserStatus Status { get; private set; }

        public ICollection<UserTenant> UserTenants { get; private set; } = new List<UserTenant>();

        protected ApplicationUser() { }

        public ApplicationUser(string fullName, string mobileNumber, string email, bool deletable = true)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            PhoneNumber = mobileNumber;
            Email = email;
            UserName = email;
            Status = UserStatus.Pending;
            Deletable = deletable;  // Set the Deletable property based on the constructor parameter
        }

        // Change user status
        public void ChangeStatus(UserStatus status)
        {
            Status = status;
        }
    }

    public enum UserStatus
    {
        Pending,
        Active,
        Banned
    }
}
