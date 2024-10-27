using PMS.Application.UseCases.Tenant.Models;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Invitations.Models;

public class InvitationDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime? CanceledAt { get; set; }

    public InvitationStatus Status { get; set; }
    
    public TenantDto Tenant { get; set; }
    // Add other relevant fields
}