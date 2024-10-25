using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions; // Assuming your RequiredTenant is here

namespace PMS.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        
        var authGroup = app.MapGroup("/api/auth")
            .WithTags("Authentication");
        
        // Register endpoint (public, no tenantEntity required)
        authGroup.MapPost("/register", [AllowAnonymous] async (RegisterDto registerDto, IAuthService authService) =>
        {
            await authService.RegisterAsync(registerDto);
            return Results.Ok("Registration successful.");
        });

        // Login endpoint (tenantEntity required)
        authGroup.MapPost("/login", [AllowAnonymous] async (LoginDto loginDto, IAuthService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }

            var response = await authService.LoginAsync(loginDto, tenantAccessor.Tenant);
            return Results.Ok(response);
        }).RequiredTenant();  

        // Profile endpoint (tenantEntity required)
        authGroup.MapGet("/profile",
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
                var userProfile = await authService.GetUserProfileAsync(userId, tenantAccessor.Tenant);
                return Results.Ok(userProfile);
            }).RequiredTenant();  

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
