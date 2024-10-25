namespace PMS.Application.UseCases.Auth.Models
{
    public class AuthLoginDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginWithRefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }
}
