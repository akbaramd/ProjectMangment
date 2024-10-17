using PMS.Application.DTOs;

namespace PMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId);
        Task<AuthResponseDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto);
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    }
}