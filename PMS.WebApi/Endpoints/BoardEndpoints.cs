using Bonyan.DomainDrivenDesign.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.UseCases.Boards;
using PMS.Application.UseCases.Boards.Models;

namespace PMS.WebApi.Endpoints
{
    public static class BoardEndpoints
    {
        public static WebApplication MapBoardEndpoints(this WebApplication app)
        {
            var boardGroup = app.MapGroup("/api/boards")
                .WithTags("Boards");

            // Get paginated boards by filter (tenantEntity required)
            boardGroup.MapGet("/", [Authorize] async (
                [FromQuery] int take,
                [FromQuery] int skip,
                [FromQuery] string? search,
                [FromQuery] Guid? sprintId,
                [FromServices] IBoardService boardService) =>
            {
                var dto = new BorderFilterDto
                {
                    Take = take,
                    Skip = skip,
                    Search = search,
                    SprintId = sprintId
                };

                var paginatedBoards = await boardService.GetBoards(dto);
                return Results.Ok(paginatedBoards);
            })
            .Produces<PaginatedResult<BoardDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            // Get board details by board ID (tenantEntity required)
            boardGroup.MapGet("/{boardId:guid}", [Authorize] async (
                Guid boardId,
                [FromServices] IBoardService boardService) =>
            {
                var boardDetails = await boardService.GetBoardDetailsAsync(boardId);

                if (boardDetails == null)
                {
                    return Results.NotFound("Board not found.");
                }

                return Results.Ok(boardDetails);
            })
            .Produces<BoardDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request
            .Produces(StatusCodes.Status404NotFound) // Board not found
            .RequiredTenant();

            // Create a new board (tenantEntity required)
            boardGroup.MapPost("/", [Authorize] async (
                [FromBody] BoardCreateDto createBoardDto,
                [FromServices] IBoardService boardService) =>
            {
                var createdBoard = await boardService.CreateBoardAsync(createBoardDto);
                return Results.Ok(createdBoard);
            })
            .Produces<BoardDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenantEntity
            .RequiredTenant();

            // Update an existing board (tenantEntity required)
            boardGroup.MapPut("/{boardId:guid}", [Authorize] async (
                Guid boardId,
                [FromBody] BoardUpdateDto updateBoardDto,
                [FromServices] IBoardService boardService) =>
            {
                var updatedBoard = await boardService.UpdateBoardAsync(boardId, updateBoardDto);

                if (updatedBoard == null)
                {
                    return Results.NotFound("Board not found or not authorized to update.");
                }

                return Results.Ok(updatedBoard);
            })
            .Produces<BoardDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenantEntity
            .Produces(StatusCodes.Status404NotFound) // Board not found
            .RequiredTenant();

            // Delete a board (tenantEntity required)
            boardGroup.MapDelete("/{boardId:guid}", [Authorize] async (
                Guid boardId,
                [FromServices] IBoardService boardService) =>
            {
                var result = await boardService.DeleteBoardAsync(boardId);

                if (!result)
                {
                    return Results.NotFound("Board not found or not authorized to delete.");
                }

                return Results.Ok("Board deleted successfully.");
            })
            .Produces(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenantEntity
            .Produces(StatusCodes.Status404NotFound) // Board not found
            .RequiredTenant();

// Update an existing board (tenantEntity required)
            boardGroup.MapPut("/{boardId:guid}/columns/{columnId:guid}", [Authorize] async (
                Guid boardId,
                Guid columnId,
                [FromBody] BoardColumnUpdateDto updateBoardColumnDto,
                [FromServices] IBoardService boardService) =>
            {
                var updatedBoard = await boardService.UpdateColumnOrderAsync(boardId, columnId,updateBoardColumnDto);

                if (updatedBoard == null)
                {
                    return Results.NotFound("Board not found or not authorized to update.");
                }

                return Results.Ok(updatedBoard);
            })
            .Produces<BoardDto>(StatusCodes.Status200OK) // Response model for success
            .Produces(StatusCodes.Status400BadRequest) // Invalid request or tenantEntity
            .Produces(StatusCodes.Status404NotFound) // Board not found
            .RequiredTenant();

            return app;
        }
    }
}
