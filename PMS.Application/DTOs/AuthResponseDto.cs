namespace PMS.Application.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } // New property
        public string UserId { get; set; }
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
    }
}
