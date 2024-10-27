using System.Security.Cryptography;
using Bonyan.DomainDrivenDesign.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Domain.BoundedContexts.UserManagment
{
    public class ApplicationUser : IdentityUser<Guid>,IEntity<Guid>
    {
        public string FullName { get; private set; }
        public bool Deletable { get; private set; } = true;
        public bool IsPhoneNumberConfirmed
        {
            get => PhoneNumberConfirmed;
            set => PhoneNumberConfirmed = value;
        }

        public UserStatus Status { get; private set; }
        public virtual ICollection<TenantMemberEntity> UserTenants { get; private set; } = new List<TenantMemberEntity>();
        
        // Refresh token properties
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpiryTime { get; private set; }

        // Failed login tracking properties
        public int FailedLoginAttempts { get; private set; } = 0;
        public DateTime? LockoutEndTime { get; private set; }

        protected ApplicationUser() { }

        public ApplicationUser(string fullName, string mobileNumber, string email, bool deletable = true)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            PhoneNumber = mobileNumber;
            Email = email;
            UserName = email;
            Status = UserStatus.Pending;
            Deletable = deletable;
        }

        // Change user status
        public void ChangeStatus(UserStatus status)
        {
            Status = status;
        }

        // Method to generate refresh token
        public void GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                RefreshToken = Convert.ToBase64String(randomNumber);
            }
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }

        // Handle failed login attempts
        public void RecordFailedLogin()
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts % 3 == 0)  // Block after every 3 failed attempts
            {
                var lockoutTime = Math.Pow(2, FailedLoginAttempts / 3 - 1); // Double the lockout time
                LockoutEndTime = DateTime.UtcNow.AddMinutes(lockoutTime);
            }
        }

        // Reset failed attempts and lockout
        public void ResetFailedLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LockoutEndTime = null;
        }

        // Check if the user is currently locked out
        public bool IsLockedOut()
        {
            return LockoutEndTime.HasValue && LockoutEndTime.Value > DateTime.UtcNow;
        }

        public object[] GetKeys()
        {
            return [Id];
        }
    }

    public enum UserStatus
    {
        Pending,
        Active,
        Banned
    }
}
