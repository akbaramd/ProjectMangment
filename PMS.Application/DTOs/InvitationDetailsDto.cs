namespace PMS.Application.DTOs
{
    public class InvitationDetailsDto
    {
        public string PhoneNumber { get; set; }
        public string TenantId { get; set; }
        public bool UserExists { get; set; }
    }
}
