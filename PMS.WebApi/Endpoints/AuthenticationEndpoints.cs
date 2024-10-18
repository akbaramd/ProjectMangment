using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions; // Assuming your RequiredTenant is here

namespace PMS.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        // Register endpoint (public, no tenant required)
        app.MapPost("/api/auth/register", [AllowAnonymous] async (RegisterDto registerDto, IAuthService authService) =>
        {
            await authService.RegisterAsync(registerDto);
            return Results.Ok("Registration successful.");
        });

        // Login endpoint (tenant required)
        app.MapPost("/api/auth/login", [AllowAnonymous] async (LoginDto loginDto, IAuthService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }
            
            var response = await authService.LoginAsync(loginDto, tenantAccessor.Tenant);
            return Results.Ok(response);
        }).RequiredTenant();  


        app.MapGet("/api/auth/profile",
            [Authorize] async (ClaimsPrincipal user, IAuthService authService, ITenantAccessor tenantAccessor) =>
            {
                
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }
                
                // Extract the user ID from the JWT claims
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Results.Unauthorized();
                }

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Results.BadRequest("Invalid user ID.");
                }

                // Call the service to get the user profile
                var userProfile = await authService.GetUserProfileAsync(userId,tenantAccessor.Tenant);
                return Results.Ok(userProfile);
            }).RequiredTenant();  

        return app;
    }
}
