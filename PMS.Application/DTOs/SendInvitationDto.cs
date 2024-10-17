namespace PMS.Application.DTOs
{
    public class SendInvitationDto
    {
        public string PhoneNumber { get; set; }
        public TimeSpan ExpirationDuration { get; set; }
    }
}
