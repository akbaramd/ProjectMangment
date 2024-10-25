using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.UseCases.Tasks;
using PMS.Application.UseCases.Tasks.Models;
using SharedKernel.Model;

namespace PMS.WebApi.Endpoints
{
    public static class TaskEndpoints
    {
        public static WebApplication MapTaskEndpoints(this WebApplication app)
        {
            // Create a group for /api/tasks
            var taskGroup = app.MapGroup("/api/tasks")
                .WithTags("Tasks");

            // Create a new task
            taskGroup.MapPost("/", [Authorize] async (
                [FromBody] TaskCreateDto createTaskDto,
                [FromServices] ITaskService taskService) =>
            {
                var task = await taskService.CreateTaskAsync(createTaskDto);
                return Results.Ok(task);
            })
            .Produces<TaskDto>();

            // Get list of tasks
            taskGroup.MapGet("/sprints/{sprintId:guid}", [Authorize] async (
                    Guid sprintId,
                [FromQuery] int take,
                [FromQuery] int skip,
                [FromQuery] string? search,
                [FromQuery] Guid? borderId,
                [FromQuery] Guid? columnId,
                [FromServices] ITaskService taskService) =>
            {
                var filter = new TaskFilterDto
                {
                    Take = take,
                    Search = search,
                    Skip = skip,
                    SprintId = sprintId,
                    BoardId = borderId,
                    ColumnId = columnId
                };
                var tasks = await taskService.GetTasksAsync(filter);
                return Results.Ok(tasks);
            })
            .Produces<PaginatedResult<TaskDto>>();

            // Get task details by ID
            taskGroup.MapGet("/{taskId:guid}", [Authorize] async (
                Guid taskId,
                [FromServices] ITaskService taskService) =>
            {
                var taskDetails = await taskService.GetTaskDetailsAsync(taskId);
                return Results.Ok(taskDetails);
            })
            .Produces<TaskDto>();

            // Update a task
            taskGroup.MapPut("/{taskId:guid}", [Authorize] async (
                Guid taskId,
                [FromBody] TaskUpdateDto updateTaskDto,
                [FromServices] ITaskService taskService) =>
            {
                var updatedTask = await taskService.UpdateTaskAsync(taskId, updateTaskDto);
                if (updatedTask == null)
                {
                    return Results.NotFound("Task not found or not authorized to update.");
                }

                return Results.Ok(updatedTask);
            })
            .Produces<TaskDto>();

            // Delete a task
            taskGroup.MapDelete("/{taskId:guid}", [Authorize] async (
                Guid taskId,
                [FromServices] ITaskService taskService) =>
            {
                var result = await taskService.DeleteTaskAsync(taskId);
                if (!result)
                {
                    return Results.NotFound("Task not found or not authorized to delete.");
                }

                return Results.Ok("Task deleted successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            // ----- Comment Endpoints -----

            // Add a comment to a task
            taskGroup.MapPost("/{taskId:guid}/comments", [Authorize] async (
                Guid taskId,
                [FromBody] TaskCommentCreateDto createCommentDto,
                [FromServices] ITaskService taskService) =>
            {
                await taskService.AddCommentAsync(taskId, createCommentDto);
                return Results.Ok("Comment added successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            // Edit a comment on a task
            taskGroup.MapPut("/{taskId:guid}/comments/{commentId:guid}", [Authorize] async (
                Guid taskId,
                Guid commentId,
                [FromBody] TaskCommentUpdateDto updateCommentDto,
                [FromServices] ITaskService taskService) =>
            {
                await taskService.EditCommentAsync(taskId, commentId, updateCommentDto);
                return Results.Ok("Comment updated successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            // Delete a comment on a task
            taskGroup.MapDelete("/{taskId:guid}/comments/{commentId:guid}", [Authorize] async (
                Guid taskId,
                Guid commentId,
                [FromServices] ITaskService taskService) =>
            {
                await taskService.DeleteCommentAsync(taskId, commentId);
                return Results.Ok("Comment deleted successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            // ----- Member Assignment Endpoints -----

            // Assign a member to a task
            taskGroup.MapPost("/{taskId:guid}/members/{memberId:guid}/assign", [Authorize] async (
                Guid taskId,
                Guid memberId,
                [FromServices] ITaskService taskService) =>
            {
                await taskService.AssignsMemberAsync(taskId, memberId);
                return Results.Ok("Member assigned successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            // Unassign a member from a task
            taskGroup.MapDelete("/{taskId:guid}/members/{memberId:guid}/unassign", [Authorize] async (
                Guid taskId,
                Guid memberId,
                [FromServices] ITaskService taskService) =>
            {
                await taskService.UnAssignsMemberAsync(taskId, memberId);
                return Results.Ok("Member unassigned successfully.");
            })
            .Produces(StatusCodes.Status200OK);

            return app;
        }
    }
}
