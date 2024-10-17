namespace PMS.Application.DTOs;

public class InvitationDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsCanceled { get; set; }
    public DateTime? CanceledAt { get; set; }
    // Add other relevant fields
}