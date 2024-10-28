using Microsoft.AspNetCore.Authorization;
using PMS.Application.UseCases.Tenants;
using PMS.Application.UseCases.Tenants.Models;

namespace PMS.WebApi.Endpoints
{
    public static class TenantRoleEndpoints
    {
        public static WebApplication MapTenantRolesEndpoints(this WebApplication app)
        {
            // Create a group for /api/tenants
            var tenantGroup = app.MapGroup("/api/tenantEntity-roles")
                .WithTags("TenantRoles"); // Add Swagger tag

                   
        // Add a new role (tenantEntity required)
        tenantGroup.MapPost("/", 
            [Authorize] async (TenantRoleCreateDto createRoleDto, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenants is required.");
            }

            await authService.AddRoleAsync(tenantAccessor.Tenant, createRoleDto);
            return Results.Ok("Role added successfully.");
        }).RequiredTenant();  

        // Update an existing role (tenantEntity required)
        tenantGroup.MapPut("/{roleId:guid}", 
            [Authorize] async (Guid roleId, TenantRoleUpdateDto updateRoleDto, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenants is required.");
            }

            await authService.UpdateRoleAsync(tenantAccessor.Tenant, roleId, updateRoleDto);
            return Results.Ok("Role updated successfully.");
        }).RequiredTenant();  

        // Delete a role (tenantEntity required)
        tenantGroup.MapDelete("/{roleId:guid}", 
            [Authorize] async (Guid roleId, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenants is required.");
            }

            await authService.DeleteRoleAsync(tenantAccessor.Tenant, roleId);
            return Results.Ok("Role deleted successfully.");
        }).RequiredTenant();  

        // Get all roles for a tenantEntity (tenantEntity required)
        tenantGroup.MapGet("/", 
            [Authorize] async (ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenants is required.");
            }

            var roles = await authService.GetRolesForTenantAsync(tenantAccessor.Tenant);
            return Results.Ok(roles);
        }).RequiredTenant();  

        // Get permission groups (no tenantEntity required, global configuration)
        tenantGroup.MapGet("/permissions", 
            [Authorize] async (ITenantRoleService authService) =>
        {
            var permissionGroups = await authService.GetPermissionGroupsAsync();
            return Results.Ok(permissionGroups);
        });


            return app;
        }
    }
}
