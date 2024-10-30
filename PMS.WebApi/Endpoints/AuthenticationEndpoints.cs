using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PMS.Application.UseCases.Auth;
using PMS.Application.UseCases.Auth.Models;

// Assuming your RequiredTenant is here

namespace PMS.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        
        var authGroup = app.MapGroup("/api/auth")
            .WithTags("Authentication");
        
        // Register endpoint (public, no tenantEntity required)
        authGroup.MapPost("/register", [AllowAnonymous] async (AuthRegisterDto registerDto, IAuthService authService) =>
        {
            await authService.RegisterAsync(registerDto);
            return Results.Ok("Registration successful.");
        });

        // Login endpoint (tenantEntity required)
        authGroup.MapPost("/login", [AllowAnonymous]
            async (AuthLoginDto loginDto, IAuthService authService) =>
            {
               

                var response = await authService.LoginAsync(loginDto);
                return Results.Ok(response);
            });

        // Profile endpoint (tenantEntity required)
        authGroup.MapGet("/profile",
            [Authorize] async (ClaimsPrincipal user, IAuthService authService) =>
            {


                // Extract the userEntity ID from the JWT claims
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Results.Unauthorized();
                }

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Results.BadRequest("Invalid userEntity ID.");
                }

                // Call the service to get the userEntity profile
                var userProfile = await authService.GetUserProfileAsync(userId);
                return Results.Ok(userProfile);
            });

        // Roles and Permissions Endpoints

        // Refresh token endpoint (public, no tenantEntity required)
        authGroup.MapPost("/refresh-token", [AllowAnonymous] async (LoginWithRefreshTokenDto refreshTokenDto, IAuthService authService) =>
        {
            var response = await authService.RefreshTokenAsync(refreshTokenDto);
            return Results.Ok(response);
        });

        return app;
    }
}
