using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Model;
using SharedKernel.Tenants.Abstractions;

namespace PMS.WebApi.Endpoints
{
    public static class ProjectEndpoints
    {
        public static WebApplication MapProjectEndpoints(this WebApplication app)
        {
            // Create a group for /api/projects
            var projectGroup = app.MapGroup("/api/projects")
                .WithTags("Projects");

            // Create a new project (tenant required)
            projectGroup.MapPost("/", [Authorize] async (
                [FromBody] CreateProjectDto createProjectDto,
                [FromServices] IProjectService projectService) =>
            {
                var project = await projectService.CreateProjectAsync(createProjectDto);
                return Results.Ok(project);
            })
            .Produces<ProjectDto>()
            .RequiredTenant();

            // Get list of projects for the tenant (tenant required)
            projectGroup.MapGet("/", [Authorize] async (
                [FromQuery] int take,
                [FromQuery] int skip,
                [FromQuery] string? search,
                [FromServices] IProjectService projectService) =>
            {
                var projects = await projectService.GetProjectsAsync(new ProjectFilterDto(take, skip, search));
                return Results.Ok(projects);
            })
            .Produces<PaginatedResult<ProjectDto>>()
            .RequiredTenant();

            // Get project details by ID (tenant required)
            projectGroup.MapGet("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] IProjectService projectService) =>
            {
                var projectDetails = await projectService.GetProjectDetailsAsync(projectId);
                return Results.Ok(projectDetails);
            })
            .Produces<ProjectDetailDto>()
            .RequiredTenant();

            // Update a project (tenant required)
            projectGroup.MapPut("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromBody] UpdateProjectDto updateProjectDto,
                [FromServices] IProjectService projectService) =>
            {
                var updatedProject = await projectService.UpdateProjectAsync(projectId, updateProjectDto);
                if (updatedProject == null)
                {
                    return Results.NotFound("Project not found or not authorized to update.");
                }

                return Results.Ok(updatedProject);
            })
            .Produces<ProjectDto>()
            .RequiredTenant();

            // Delete a project (tenant required)
            projectGroup.MapDelete("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] IProjectService projectService) =>
            {
                var result = await projectService.DeleteProjectAsync(projectId);
                if (!result)
                {
                    return Results.NotFound("Project not found or not authorized to delete.");
                }

                return Results.Ok("Project deleted successfully.");
            })
            .Produces(StatusCodes.Status200OK)
            .RequiredTenant();

            return app;
        }
    }
}
