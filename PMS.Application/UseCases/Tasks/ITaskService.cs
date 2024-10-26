using PMS.Application.UseCases.Tasks.Models;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Tasks
{
    public interface ITaskService
    {
        Task<PaginatedResult<TaskDto>> GetTasksAsync(TaskFilterDto filter);
        Task<TaskDto> GetTaskDetailsAsync(Guid taskId);
        Task<TaskDto> CreateTaskAsync(TaskCreateDto taskCreateDto);
        Task<bool> DeleteTaskAsync(Guid taskId);
        Task<TaskDto?> UpdateTaskAsync(Guid taskId, TaskUpdateDto taskUpdateDto);


        Task AddCommentAsync(Guid taskId, TaskCommentCreateDto createDto);
        Task EditCommentAsync(Guid taskId, Guid commentId, TaskCommentUpdateDto createDto);
        Task DeleteCommentAsync(Guid taskId, Guid commentId);
        
        Task AssignMemberAsync(Guid taskId, Guid projectMemberId);
        Task UnassignMemberAsync(Guid taskId, Guid projectMemberId);
    }
}