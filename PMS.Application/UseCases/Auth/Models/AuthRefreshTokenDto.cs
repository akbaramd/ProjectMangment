namespace PMS.Application.UseCases.Auth.Models;

public class AuthRefreshTokenDto
{
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
}