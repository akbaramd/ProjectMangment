using Bonyan.DomainDrivenDesign.Domain.Enumerations;
using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.TenantManagement
{
    public class ProjectInvitationEntity : TenantEntityBase
    {
        public string PhoneNumber { get; private set; } = null!;  // Non-nullable property initialization
        public DateTime SentAt { get; private set; }
        public DateTime? AcceptedAt { get; private set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? CanceledAt { get; set; }
        public InvitationStatus Status { get; private set; }

        // Parameterless constructor for EF Core
        protected ProjectInvitationEntity()  // Call base constructor with a dummy value
        {
            // Initialize non-nullable properties for EF Core
        }

        public ProjectInvitationEntity(string phoneNumber,TimeSpan expirationDuration)
     // Use the real tenant when using this constructor in your business logic
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
            Status = InvitationStatus.Canceled;
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

 
    
    public class InvitationStatus : Enumeration
    {
        public static InvitationStatus Pending = new InvitationStatus(0, nameof(Pending)); 
        public static InvitationStatus Accepted = new InvitationStatus(1, nameof(Accepted)); 
        public static InvitationStatus Rejected = new InvitationStatus(2, nameof(Rejected)); 
        public static InvitationStatus Canceled = new InvitationStatus(3 ,nameof(Canceled)); 
        public InvitationStatus(int id, string name) : base(id, name)
        {
        }
    }
}
