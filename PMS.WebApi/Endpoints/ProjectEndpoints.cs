using System.Security.Claims;
using Bonyan.DomainDrivenDesign.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.UseCases.Projects;
using PMS.Application.UseCases.Projects.Models;

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
                [FromBody] ProjectCreateDto createProjectDto,
                [FromServices] ClaimsPrincipal FromServices,
                [FromServices] IProjectService projectService) =>
                {
                    var id = Guid.Parse(FromServices.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                var project = await projectService.CreateProjectAsync(createProjectDto,id);
                return Results.Ok(project);
            })
            .Produces<ProjectDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

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
            .Produces<PaginatedResult<ProjectDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // Get project details by ID (tenant required)
            projectGroup.MapGet("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] IProjectService projectService) =>
            {
                var projectDetails = await projectService.GetProjectDetailsAsync(projectId);
                return Results.Ok(projectDetails);
            })
            .Produces<ProjectDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // Update a project (tenant required)
            projectGroup.MapPut("/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromBody] ProjectUpdateDto updateProjectDto,
                [FromServices] IProjectService projectService) =>
            {
                var updatedProject = await projectService.UpdateProjectAsync(projectId, updateProjectDto);
                if (updatedProject == null)
                {
                    return Results.NotFound("Project not found or not authorized to update.");
                }

                return Results.Ok(updatedProject);
            })
            .Produces<ProjectDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

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
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // ----- Member Endpoints -----

            // Add a member to a project
            projectGroup.MapPost("/{projectId:guid}/members", [Authorize] async (
                Guid projectId,
                [FromBody] ProjectAddMemberDto addMemberDto,
                [FromServices] IProjectService projectService) =>
            {
                var member = await projectService.AddMemberAsync(projectId, addMemberDto);
                return Results.Ok(member);
            })
            .Produces<ProjectMemberDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // Remove a member from a project
            projectGroup.MapDelete("/{projectId:guid}/members/{memberId:guid}", [Authorize] async (
                Guid projectId,
                Guid memberId,
                [FromServices] IProjectService projectService) =>
            {
                var result = await projectService.RemoveMemberAsync(projectId, memberId);
                if (!result)
                {
                    return Results.NotFound("Member not found or not authorized to remove.");
                }

                return Results.Ok("Member removed successfully.");
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // Get members of a project
            projectGroup.MapGet("/{projectId:guid}/members", [Authorize] async (
                Guid projectId,
                [FromQuery] int take,
                [FromQuery] int skip,
                [FromQuery] string? search,
                [FromServices] IProjectService projectService) =>
            {
                var filter = new ProjectMemberFilterDto(take, skip, search);
                var members = await projectService.GetMembersAsync(projectId, filter);
                return Results.Ok(members);
            })
            .Produces<PaginatedResult<ProjectMemberDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            // Update a project member
            projectGroup.MapPut("/{projectId:guid}/members/{memberId:guid}", [Authorize] async (
                Guid projectId,
                Guid memberId,
                [FromBody] ProjectUpdateMemberDto updateMemberDto,
                [FromServices] ClaimsPrincipal FromServices,
                [FromServices] IProjectService projectService) =>
            {
                var updatedMember = await projectService.UpdateMemberAsync(projectId, memberId, updateMemberDto);
                return Results.Ok(updatedMember);
            })
            .Produces<ProjectMemberDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
       ;

            return app;
        }
    }
}
