namespace PMS.Application.DTOs
{
    public class InvitationDetailsDto
    {
        public string Email { get; set; }
        public string TenantId { get; set; }
        public bool UserExists { get; set; }
    }
}
