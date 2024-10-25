using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;
using System.Security.Claims;

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
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                var tenantInfo = await tenantService.GetTenantInfoAsync(tenantAccessor.Tenant, userId);
                return Results.Ok(tenantInfo);
            })
            .RequiredTenant();

            // Remove a member from tenant (tenant required)
            tenantGroup.MapDelete("/members/{memberId:guid}", [Authorize] async (
                Guid memberId,
                [FromServices] ITenantService tenantService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                await tenantService.RemoveMemberAsync(tenantAccessor.Tenant, userId, memberId);
                return Results.Ok("Member removed successfully.");
            })
            .RequiredTenant();

            // Update member role in tenant (tenant required)
            tenantGroup.MapPut("/members/{memberId:guid}/role", [Authorize] async (
                Guid memberId,
                [FromBody] TenantMemberUpdate dto,
                [FromServices] ITenantService tenantService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                await tenantService.UpdateMemberRoleAsync(tenantAccessor.Tenant, userId, memberId, dto.Role);
                return Results.Ok("Member role updated successfully.");
            })
            .RequiredTenant();

            // Get members of a tenant (only for authorized roles like Owner, Manager, Administrator)
            tenantGroup.MapGet("/members", [Authorize] async (
                    [FromQuery] int take,
                    [FromQuery] int skip,
                    [FromQuery] string? search,
                    [FromQuery] string? sortDirection,
                    [FromQuery] string? sortBy,
                [FromServices] ITenantService tenantService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                var tenantInfo = await tenantService.GetMembers(tenantAccessor.Tenant, userId,new TenantMembersFilterDto(take,skip,search,sortBy,sortDirection));

                return Results.Ok(tenantInfo);
            })
            .RequiredTenant();

            return app;
        }
    }
}
