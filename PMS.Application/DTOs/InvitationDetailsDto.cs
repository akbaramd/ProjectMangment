namespace PMS.Application.DTOs
{
    public class InvitationDetailsDto
    {
        public InvitationDto Invitation { get; set; }
        public bool UserExists { get; set; }
    }
}
