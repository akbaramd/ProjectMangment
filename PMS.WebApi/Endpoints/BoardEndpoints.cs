using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;
using System.Security.Claims;

namespace PMS.WebApi.Endpoints
{
    public static class BoardEndpoints
    {
        public static WebApplication MapBoardEndpoints(this WebApplication app)
        {
            var boardGroup = app.MapGroup("/api/boards")
                .WithTags("Boards");

            // Get boards by sprint ID, including columns (tenant required)
            boardGroup.MapGet("/", [Authorize] async (
                Guid sprintId,
                [FromServices] IBoardService boardService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var tenantId = tenantAccessor.Tenant;

                var boards = await boardService.GetBoardsBySprintIdAsync(sprintId, tenantId, userId);
                return Results.Ok(boards);
            })
            .Produces<List<BoardDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            // Get board details by board ID (tenant required)
            boardGroup.MapGet("/{boardId:guid}", [Authorize] async (
                Guid boardId,
                [FromServices] IBoardService boardService,
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

                // Get board details for the given board ID
                var boardDetails = await boardService.GetBoardDetailsAsync(boardId, tenantId, userId);

                if (boardDetails == null)
                {
                    return Results.NotFound("Board not found or not authorized.");
                }

                return Results.Ok(boardDetails);
            })
            .Produces<BoardDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenant
            .Produces(StatusCodes.Status404NotFound) // Board not found
            .RequiredTenant();

            return app;
        }
    }
}
