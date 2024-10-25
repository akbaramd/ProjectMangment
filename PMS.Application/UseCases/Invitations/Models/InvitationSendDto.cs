namespace PMS.Application.UseCases.Invitations.Models
{
    public class InvitationSendDto
    {
        public string PhoneNumber { get; set; }
        public TimeSpan ExpirationDuration { get; set; }
    }
}
