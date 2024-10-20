﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;
using System.Security.Claims;

namespace PMS.WebApi.Endpoints
{
    public static class TenantRoleEndpoints
    {
        public static WebApplication MapTenantRolesEndpoints(this WebApplication app)
        {
            // Create a group for /api/tenants
            var tenantGroup = app.MapGroup("/api/tenant-roles")
                .WithTags("TenantRoles"); // Add Swagger tag

                   
        // Add a new role (tenant required)
        tenantGroup.MapPost("/", 
            [Authorize] async (CreateRoleDto createRoleDto, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }

            await authService.AddRoleAsync(tenantAccessor.Tenant, createRoleDto);
            return Results.Ok("Role added successfully.");
        }).RequiredTenant();  

        // Update an existing role (tenant required)
        tenantGroup.MapPut("/{roleId:guid}", 
            [Authorize] async (Guid roleId, UpdateRoleDto updateRoleDto, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }

            await authService.UpdateRoleAsync(tenantAccessor.Tenant, roleId, updateRoleDto);
            return Results.Ok("Role updated successfully.");
        }).RequiredTenant();  

        // Delete a role (tenant required)
        tenantGroup.MapDelete("/{roleId:guid}", 
            [Authorize] async (Guid roleId, ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }

            await authService.DeleteRoleAsync(tenantAccessor.Tenant, roleId);
            return Results.Ok("Role deleted successfully.");
        }).RequiredTenant();  

        // Get all roles for a tenant (tenant required)
        tenantGroup.MapGet("/", 
            [Authorize] async (ITenantRoleService authService, ITenantAccessor tenantAccessor) =>
        {
            if (tenantAccessor.Tenant == null)
            {
                return Results.BadRequest("Tenant is required.");
            }

            var roles = await authService.GetRolesForTenantAsync(tenantAccessor.Tenant);
            return Results.Ok(roles);
        }).RequiredTenant();  

        // Get permission groups (no tenant required, global configuration)
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
