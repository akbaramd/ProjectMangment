using PMS.Application.UseCases.Auth.Models;
using PMS.Application.UseCases.User.Models;

namespace PMS.Application.UseCases.Auth;

public interface IAuthService
{
    Task RegisterAsync(AuthRegisterDto authRegisterDto);
    Task<AuthJwtDto> LoginAsync(AuthLoginDto authLoginDto);
    Task<AuthJwtDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto);
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);


}