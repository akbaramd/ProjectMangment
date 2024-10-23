using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;

namespace PMS.WebApi.Endpoints
{
    public static class InvitationEndpoints
    {
        public static WebApplication MapInvitationEndpoints(this WebApplication app)
        {
            // Create a group for /api/invitations
            var invitationsGroup = app.MapGroup("/api/invitations")
                .WithTags("Invitations"); // Add Swagger tag

            // Get all invitations (tenant required)
            invitationsGroup.MapGet("/", [Authorize] async (
                    [FromQuery] int take,
                    [FromQuery] int skip,
                    [FromQuery] string? search,
                    [FromQuery] string? sortDirection,
                    [FromQuery] string? sortBy,
                [FromServices] IInvitationService invitationService,
                [FromServices] ITenantAccessor tenantAccessor) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

               

                var invitations = await invitationService.GetAllInvitationsAsync(new InvitationFilterDto(take,skip,search,sortBy,sortDirection), tenantAccessor.Tenant);
                return Results.Ok(invitations);
            })
            .RequiredTenant();

            // Get invitation detail (no tenant required)
            invitationsGroup.MapGet("/{invitationId:guid}", [AllowAnonymous] async (Guid invitationId, [FromServices] IInvitationService invitationService) =>
            {
                var invitationDetails = await invitationService.GetInvitationDetailsAsync(invitationId);
                return Results.Ok(invitationDetails);
            });

            // Send invitation (tenant required)
            invitationsGroup.MapPost("/", [Authorize] async ([FromBody] SendInvitationDto sendInvitationDto,ClaimsPrincipal user, [FromServices] IInvitationService invitationService, [FromServices] ITenantAccessor tenantAccessor) =>
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
                await invitationService.SendInvitationAsync(sendInvitationDto, tenantAccessor.Tenant,userId);
                return Results.Ok("Invitation sent successfully.");
            })
            .RequiredTenant();

            // Accept invitation (no tenant required)
            invitationsGroup.MapPost("/{invitationId:guid}/accept", [AllowAnonymous] async (Guid invitationId, [FromServices] IInvitationService invitationService) =>
            {
                await invitationService.AcceptInvitationAsync(invitationId);
                return Results.Ok("Invitation accepted.");
            });

            // Reject invitation (no tenant required)
            invitationsGroup.MapPost("/{invitationId:guid}/reject", [AllowAnonymous] async (Guid invitationId, [FromServices] IInvitationService invitationService) =>
            {
                await invitationService.RejectInvitationAsync(invitationId);
                return Results.Ok("Invitation rejected.");
            });

            // Cancel invitation (tenant required)
            invitationsGroup.MapPost("/{invitationId:guid}/cancel", [Authorize] async (Guid invitationId,ClaimsPrincipal user, [FromServices] IInvitationService invitationService, [FromServices] ITenantAccessor tenantAccessor) =>
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
                await invitationService.CancelInvitationAsync(invitationId, tenantAccessor.Tenant,userId);
                return Results.Ok("Invitation canceled.");
            })
            .RequiredTenant();

            // Resend invitation (tenant required)
            invitationsGroup.MapPost("/{invitationId:guid}/resend", [Authorize] async (Guid invitationId, [FromServices] IInvitationService invitationService, [FromServices] ITenantAccessor tenantAccessor) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                await invitationService.ResendInvitationAsync(invitationId, tenantAccessor.Tenant);
                return Results.Ok("Invitation resent successfully.");
            })
            .RequiredTenant();

            // Update invitation (tenant required)
            invitationsGroup.MapPut("/{invitationId:guid}", [Authorize] async (Guid invitationId, [FromBody] UpdateInvitationDto updateInvitationDto, [FromServices] IInvitationService invitationService, [FromServices] ITenantAccessor tenantAccessor) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                await invitationService.UpdateInvitationAsync(invitationId, updateInvitationDto, tenantAccessor.Tenant);
                return Results.Ok("Invitation updated successfully.");
            })
            .RequiredTenant();

            return app;
        }
    }
}
