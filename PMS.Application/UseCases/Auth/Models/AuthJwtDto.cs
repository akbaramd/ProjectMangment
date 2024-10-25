namespace PMS.Application.UseCases.Auth.Models
{
    public class AuthJwtDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } // New property
        public string UserId { get; set; }
    }
}
