using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bonyan.DomainDrivenDesign.Domain.Model;
using Bonyan.MultiTenant;
using PMS.Application.UseCases.Tenants;
using PMS.Application.UseCases.Tenants.Models;

namespace PMS.WebApi.Endpoints
{
    public static class TenantEndpoints
    {
        public static WebApplication MapTenantEndpoints(this WebApplication app)
        {
            // Create a group for /api/tenants
            var tenantGroup = app.MapGroup("/api/tenants")
                .WithTags("Tenants"); // Add Swagger tag

            // Get tenant info (tenant required)
            tenantGroup.MapGet("/", [Authorize] async (
                [FromServices] ITenantService tenantService,
                [FromServices] ICurrentTenant tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Name == null)
                {
                    return Results.BadRequest("Tenants is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                var tenantInfo = await tenantService.GetTenantInfoAsync(tenantAccessor.Name, userId);
                return Results.Ok(tenantInfo);
            })
            .Produces<TenantDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
     ;

            // Remove a member from tenant (tenant required)
            tenantGroup.MapDelete("/members/{memberId:guid}", [Authorize] async (
                Guid memberId,
                [FromServices] ITenantService tenantService,
                [FromServices] ICurrentTenant tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Name == null)
                {
                    return Results.BadRequest("Tenants is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                await tenantService.RemoveMemberAsync(tenantAccessor.Name, userId, memberId);
                return Results.Ok("Member removed successfully.");
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
     ;

            // Update member role in tenant (tenant required)
            tenantGroup.MapPut("/members/{memberId:guid}/role", [Authorize] async (
                Guid memberId,
                [FromBody] TenantMemberUpdate dto,
                [FromServices] ITenantService tenantService,
                [FromServices] ICurrentTenant tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Name == null)
                {
                    return Results.BadRequest("Tenants is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                await tenantService.UpdateMemberRoleAsync(tenantAccessor.Name, userId, memberId, dto.Role);
                return Results.Ok("Member role updated successfully.");
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
     ;

            // Get members of a tenant (only for authorized roles like Owner, Manager, Administrator)
            tenantGroup.MapGet("/members", [Authorize] async (
                    [FromQuery] int take,
                    [FromQuery] int skip,
                    [FromQuery] string? search, 
                    [FromQuery] string? sortDirection,
                    [FromQuery] string? sortBy,
                [FromServices] ITenantService tenantService,
                [FromServices] ICurrentTenant tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Name == null)
                {
                    return Results.BadRequest("Tenants is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                var tenantInfo = await tenantService.GetMembers(tenantAccessor.Name, userId, new TenantMembersFilterDto(take, skip, search, sortBy, sortDirection));

                return Results.Ok(tenantInfo);
            })
            .Produces<PaginatedResult<TenantMemberDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
     ;

            return app;
        }
    }
}
