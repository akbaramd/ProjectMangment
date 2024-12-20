using Bonyan.DomainDrivenDesign.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.UseCases.Sprints;
using PMS.Application.UseCases.Sprints.Models;

namespace PMS.WebApi.Endpoints
{
    public static class SprintEndpoints
    {
        public static WebApplication MapSprintEndpoints(this WebApplication app)
        {
            var sprintGroup = app.MapGroup("/api/sprints")
                .WithTags("Sprints");

            // Get sprints by project ID with pagination and filtering (tenantEntity required)
            sprintGroup.MapGet("/", [Authorize] async (
                    [FromQuery] int take,    // Pagination: page number
                    [FromQuery] int skip,      // Pagination: page size
                    [FromQuery] string? search,    // Optional search query
                    [FromQuery] Guid? projectId,    // Optional search query
                    [FromServices] ISprintService sprintService) =>
                {
                    // Create SprintFilterDto with provided pagination and search parameters
                    var filterDto = new SprintFilterDto
                    {
                        ProjectId = projectId,
                        Search = search,
                        Skip = skip,
                        Take = take
                    };

                    // Fetch paginated sprint results
                    var paginatedSprints = await sprintService.GetSprintsAsync(filterDto);
                    return Results.Ok(paginatedSprints);
                })
                .Produces<PaginatedResult<SprintDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .RequiredTenant();


            // Get sprint details by sprint ID (tenantEntity required)
            sprintGroup.MapGet("/{sprintId:guid}", [Authorize] async (
                Guid sprintId,
                [FromServices] ISprintService sprintService) =>
            {
                var sprintDetails = await sprintService.GetSprintDetailsAsync(sprintId);
                if (sprintDetails == null)
                {
                    return Results.NotFound("Sprint not found.");
                }

                return Results.Ok(sprintDetails);
            })
            .Produces<SprintDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            // Create a new sprint (tenantEntity required)
            sprintGroup.MapPost("/", [Authorize] async (
                [FromBody] SprintCreateDto createSprintDto,
                [FromServices] ISprintService sprintService) =>
            {
                var sprint = await sprintService.CreateSprintAsync(createSprintDto);
                return Results.Created($"/api/sprints/{sprint.Id}", sprint);
            })
            .Produces<SprintDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequiredTenant();

            // Update an existing sprint (tenantEntity required)
            sprintGroup.MapPut("/{sprintId:guid}", [Authorize] async (
                Guid sprintId,
                [FromBody] SprintUpdateDto updateSprintDto,
                [FromServices] ISprintService sprintService) =>
            {
                var updatedSprint = await sprintService.UpdateSprintAsync(sprintId, updateSprintDto);
                if (updatedSprint == null)
                {
                    return Results.NotFound("Sprint not found or not authorized to update.");
                }

                return Results.Ok(updatedSprint);
            })
            .Produces<SprintDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            // Delete a sprint (tenantEntity required)
            sprintGroup.MapDelete("/{sprintId:guid}", [Authorize] async (
                Guid sprintId,
                [FromServices] ISprintService sprintService) =>
            {
                var result = await sprintService.DeleteSprintAsync(sprintId);
                if (!result)
                {
                    return Results.NotFound("Sprint not found or not authorized to delete.");
                }

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            return app;
        }
    }
}
