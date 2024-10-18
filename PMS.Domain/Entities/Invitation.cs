using PMS.Domain.Core;

namespace PMS.Domain.Entities
{
    public class Invitation : TenantEntity
    {
        public string PhoneNumber { get; private set; } = null!;  // Non-nullable property initialization
        public DateTime SentAt { get; private set; }
        public DateTime? AcceptedAt { get; private set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? CanceledAt { get; set; }
        public InvitationStatus Status { get; private set; }

        // Parameterless constructor for EF Core
        protected Invitation()  // Call base constructor with a dummy value
        {
            // Initialize non-nullable properties for EF Core
        }

        public Invitation(string phoneNumber, Tenant tenant,TimeSpan expirationDuration)
            : base(tenant)  // Use the real tenant when using this constructor in your business logic
        {
            PhoneNumber = phoneNumber;
            SentAt = DateTime.UtcNow;
            Status = InvitationStatus.Pending;
            ExpirationDate = CreatedAt.Add(expirationDuration);
        }

        // Accept the invitation
        public void Accept()
        {
            if (Status== InvitationStatus.Accepted) throw new InvalidOperationException("Invitation already accepted.");
            AcceptedAt = DateTime.UtcNow;
            Status = InvitationStatus.Accepted;
        }
        
        public void Renew(TimeSpan newExpirationDuration)
        {
            if (Status== InvitationStatus.Accepted) throw new InvalidOperationException("Invitation already accepted.");
            Status = InvitationStatus.Pending;
            ExpirationDate = CreatedAt.Add(newExpirationDuration);
        }
        
        public void Cancel()
        {
            if (Status== InvitationStatus.Accepted) throw new InvalidOperationException("Invitation already accepted.");
            Status = InvitationStatus.Cancel;
            CanceledAt = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationDate;
        }
        // Reject the invitation
        public void Reject()
        {
            if (Status != InvitationStatus.Pending) throw new InvalidOperationException("Invitation already processed.");
            Status = InvitationStatus.Rejected;
        }
    }

    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancel
    }
}
