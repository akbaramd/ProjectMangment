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
    var taskGroup = app.MapGroup("/api/tasks").WithTags("Tasks");

    // Create a new task
    taskGroup.MapPost("/", [Authorize] async (
        [FromBody] TaskCreateDto createTaskDto,
        [FromServices] ITaskService taskService) =>
    {
        var task = await taskService.CreateTaskAsync(createTaskDto);
        return task != null ? Results.Created($"/api/tasks/{task.Id}", task) : Results.BadRequest("Task creation failed.");
    })
    .Produces<TaskDto>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized);

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
    .Produces<PaginatedResult<TaskDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

    // Get task details by ID
    taskGroup.MapGet("/{taskId:guid}", [Authorize] async (
        Guid taskId,
        [FromServices] ITaskService taskService) =>
    {
        var taskDetails = await taskService.GetTaskDetailsAsync(taskId);
        return taskDetails != null ? Results.Ok(taskDetails) : Results.NotFound("Task not found.");
    })
    .Produces<TaskDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized);

    // Update a task
    taskGroup.MapPut("/{taskId:guid}", [Authorize] async (
        Guid taskId,
        [FromBody] TaskUpdateDto updateTaskDto,
        [FromServices] ITaskService taskService) =>
    {
        var updatedTask = await taskService.UpdateTaskAsync(taskId, updateTaskDto);
        return updatedTask != null ? Results.Ok(updatedTask) : Results.NotFound("Task not found or update failed.");
    })
    .Produces<TaskDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized);

    // Delete a task
    taskGroup.MapDelete("/{taskId:guid}", [Authorize] async (
        Guid taskId,
        [FromServices] ITaskService taskService) =>
    {
        var result = await taskService.DeleteTaskAsync(taskId);
        return result ? Results.Ok("Task deleted successfully.") : Results.NotFound("Task not found.");
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized);

    // Add a comment to a task
    taskGroup.MapPost("/{taskId:guid}/comments", [Authorize] async (
        Guid taskId,
        [FromBody] TaskCommentCreateDto createCommentDto,
        [FromServices] ITaskService taskService) =>
    {
        await taskService.AddCommentAsync(taskId, createCommentDto);
        return Results.Ok("Comment added successfully.");
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

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
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

    // Delete a comment on a task
    taskGroup.MapDelete("/{taskId:guid}/comments/{commentId:guid}", [Authorize] async (
        Guid taskId,
        Guid commentId,
        [FromServices] ITaskService taskService) =>
    {
        await taskService.DeleteCommentAsync(taskId, commentId);
        return Results.Ok("Comment deleted successfully.");
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

    // Assign a member to a task
    taskGroup.MapPost("/{taskId:guid}/members/{memberId:guid}/assign", [Authorize] async (
        Guid taskId,
        Guid memberId,
        [FromServices] ITaskService taskService) =>
    {
        await taskService.AssignMemberAsync(taskId, memberId);
        return Results.Ok("Member assigned successfully.");
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

    // Unassign a member from a task
    taskGroup.MapDelete("/{taskId:guid}/members/{memberId:guid}/unassign", [Authorize] async (
        Guid taskId,
        Guid memberId,
        [FromServices] ITaskService taskService) =>
    {
        await taskService.UnassignMemberAsync(taskId, memberId);
        return Results.Ok("Member unassigned successfully.");
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

    return app;
}
    }
}
