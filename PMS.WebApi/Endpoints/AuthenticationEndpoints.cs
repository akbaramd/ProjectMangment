using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

public static class AuthenticationEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/register", [AllowAnonymous] async (RegisterDto registerDto, IAuthService authService, HttpContext httpContext) =>
        {
            // Use tenantId as needed
            await authService.RegisterAsync(registerDto);
            return Results.Ok("Registration successful.");
        });

        app.MapPost("/api/auth/login", [AllowAnonymous] async (LoginDto loginDto, IAuthService authService, HttpContext httpContext) =>
        {
            var tenantId = httpContext.Request.Headers["X-Tenant"].ToString();
            // Use tenantId as needed
            var response = await authService.LoginAsync(loginDto, tenantId);
            return Results.Ok(response);
        });

        app.MapPost("/api/auth/send-invitation", [AllowAnonymous] async (SendInvitationDto sendInvitationDto, IAuthService authService, HttpContext httpContext) =>
        {
            var tenantId = httpContext.Request.Headers["X-Tenant"].ToString();
            // Use tenantId as needed
            await authService.SendInvitationAsync(sendInvitationDto, tenantId);
            return Results.Ok("Invitation sent.");
        });

        app.MapGet("/api/auth/invitation-details/{invitationToken:guid}", [AllowAnonymous] async (Guid invitationToken, IAuthService authService, HttpContext httpContext) =>
        {
            var details = await authService.GetInvitationDetailsAsync(invitationToken);
            return Results.Ok(details);
        });

        app.MapPost("/api/auth/accept-invitation", [AllowAnonymous] async (AcceptInvitationDto acceptInvitationDto, IAuthService authService, HttpContext httpContext) =>
        {
            await authService.AcceptInvitationAsync(acceptInvitationDto);
            return Results.Ok("Invitation accepted.");
        });

        return app;
    }
}
