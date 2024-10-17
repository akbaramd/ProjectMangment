namespace PMS.Application.DTOs
{
    public class LoginDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginWithRefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }
}
