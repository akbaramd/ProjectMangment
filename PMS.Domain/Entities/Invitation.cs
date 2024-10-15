using PMS.Domain.Core;

namespace PMS.Domain.Entities
{
    public class Invitation : TenantEntity
    {
        public string Email { get; private set; } = null!;  // Non-nullable property initialization
        public DateTime SentAt { get; private set; }
        public bool IsAccepted { get; private set; }
        public DateTime? AcceptedAt { get; private set; }

        public InvitationStatus Status { get; private set; }

        // Parameterless constructor for EF Core
        protected Invitation()  // Call base constructor with a dummy value
        {
            // Initialize non-nullable properties for EF Core
        }

        public Invitation(string email, Tenant tenant)
            : base(tenant)  // Use the real tenant when using this constructor in your business logic
        {
            Email = email;
            SentAt = DateTime.UtcNow;
            Status = InvitationStatus.Pending;
            IsAccepted = false;
        }

        // Accept the invitation
        public void Accept()
        {
            if (IsAccepted) throw new InvalidOperationException("Invitation already accepted.");
            IsAccepted = true;
            AcceptedAt = DateTime.UtcNow;
            Status = InvitationStatus.Accepted;
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
        Rejected
    }
}
