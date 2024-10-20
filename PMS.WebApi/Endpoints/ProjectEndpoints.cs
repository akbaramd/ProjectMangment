using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;
using System.Security.Claims;

namespace PMS.WebApi.Endpoints
{
    public static class ProjectEndpoints
    {
        public static WebApplication MapProjectEndpoints(this WebApplication app)
        {
            // Create a group for /api/projects
            var projectGroup = app.MapGroup("/api/projects")
                .WithTags("Projects"); // Add Swagger tag

            // Create a new project (tenant required)
            projectGroup.MapPost("/", [Authorize] async (
                [FromBody] CreateProjectDto createProjectDto,
                [FromServices] IProjectService projectService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                // Create project
                var project = await projectService.CreateProjectAsync(createProjectDto, tenantAccessor.Tenant ,userId);
                return Results.Ok(project);
            })
            .Produces<ProjectDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .RequiredTenant();

            // Get list of projects for the tenant (tenant required)
            projectGroup.MapGet("/", [Authorize] async (
                [FromServices] IProjectService projectService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract tenant ID
                var tenantId = tenantAccessor.Tenant;

                // Get projects for tenant
                var projects = await projectService.GetProjectListAsync(tenantId);
                return Results.Ok(projects);
            })
            .Produces<List<ProjectDto>>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .RequiredTenant();

            // Get project details by ID (tenant required)
            projectGroup.MapGet("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] IProjectService projectService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract tenant ID
                var tenantId = tenantAccessor.Tenant;

                // Get project details
                var projectDetails = await projectService.GetProjectDetailsAsync(projectId, tenantId);
                return Results.Ok(projectDetails);
            })
            .Produces<ProjectDetailsDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status404NotFound) // Project not found
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .RequiredTenant();

            // Update a project (tenant required)
            projectGroup.MapPut("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromBody] UpdateProjectDto updateProjectDto,
                [FromServices] IProjectService projectService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var tenantId = tenantAccessor.Tenant;

                // Update project
                var updatedProject = await projectService.UpdateProjectAsync(projectId, updateProjectDto, tenantId, userId);
                if (updatedProject == null)
                {
                    return Results.NotFound("Project not found or not authorized to update.");
                }

                return Results.Ok(updatedProject);
            })
            .Produces<ProjectDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status404NotFound) // Project not found
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .RequiredTenant();

            // Delete a project (tenant required)
            projectGroup.MapDelete("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] IProjectService projectService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                // Extract user ID from claims
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var tenantId = tenantAccessor.Tenant;

                // Delete project
                var result = await projectService.DeleteProjectAsync(projectId, tenantId, userId);
                if (!result)
                {
                    return Results.NotFound("Project not found or not authorized to delete.");
                }

                return Results.Ok("Project deleted successfully.");
            })
            .Produces(StatusCodes.Status200OK) // Project successfully deleted
            .Produces(StatusCodes.Status404NotFound) // Project not found
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .RequiredTenant();



            return app;
        }
    }
}
