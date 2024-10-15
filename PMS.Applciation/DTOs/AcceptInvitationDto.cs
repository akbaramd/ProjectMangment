namespace PMS.Application.DTOs
{
    public class AcceptInvitationDto
    {
        public Guid InvitationToken { get; set; }
        public string UserId { get; set; }
    }
}
