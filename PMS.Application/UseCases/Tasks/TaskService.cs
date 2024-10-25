using AutoMapper;
using PMS.Application.Base;
using PMS.Application.UseCases.Projects.Exceptions;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Application.UseCases.Tasks.Specs;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.TaskManagment.Repositories;
using PMS.Domain.BoundedContexts.TenantManagment;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Tasks
{
    public class TaskService : BaseTenantService, ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public TaskService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IMapper mapper,
            IServiceProvider serviceProvider, IBoardColumnRepository boardColumnRepository) : base(serviceProvider)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _boardColumnRepository = boardColumnRepository;
        }

        public async Task<PaginatedResult<TaskDto>> GetTasksAsync(TaskFilterDto filter)
        {
            // Fetch paginated tasks with filtering and tenant validation
            var tasks = await _taskRepository.PaginatedAsync(new GetTaskBySprintSpec(filter));
            return _mapper.Map<PaginatedResult<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskDetailsAsync(Guid taskId)
        {
            // Retrieve task details
            var task = await _taskRepository.FindOneAsync(new GetTaskDetailsByIdSpec(taskId));
            if (task == null) throw new KeyNotFoundException("Task not found");

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(TaskCreateDto taskCreateDto)
        {
            // Map to entity and save


            var column = await _boardColumnRepository.FindByIdAsync(taskCreateDto.ColumnId);

            var order = column.Tasks.MaxBy(x => x.Order).Order;
            
            var entity = new TaskEntity(taskCreateDto.Title, taskCreateDto.Description, taskCreateDto.Content,
                order+1, column, CurrentTenant, taskCreateDto.DueDate);
            await _taskRepository.AddAsync(entity);
            return await GetTaskDetailsAsync(entity.Id);
        }

        public async Task<TaskDto?> UpdateTaskAsync(Guid taskId, TaskUpdateDto taskUpdateDto)
        {
            // Retrieve and update task
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            _mapper.Map(taskUpdateDto, task);
            await _taskRepository.UpdateAsync(task);
            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            // Check if task exists
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(task);
            return true;
        }

        // Comment management
        public async Task AddCommentAsync(Guid taskId, TaskCommentCreateDto createDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var tenantmember = await ValidateTenantAccessAsync("task:create");

            var projectMember =
                task.BoardColumn.Board.Sprint.Project.Members.FirstOrDefault(c => c.TenantMemberId == tenantmember.Id);
            if (projectMember == null) throw new ProjectNotFoundException("Proejct member not found");


            task.AddComment(projectMember, createDto.Content);
            await _taskRepository.UpdateAsync(task);
        }

        public async Task EditCommentAsync(Guid taskId, Guid commentId, TaskCommentUpdateDto updateDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) throw new KeyNotFoundException("Comment not found");

            comment.UpdateContent(updateDto.Content);
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteCommentAsync(Guid taskId, Guid commentId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) throw new KeyNotFoundException("Comment not found");

            await _taskRepository.DeleteAsync(task);
        }

        // Member assignment
        public async Task AssignsMemberAsync(Guid taskId, Guid projectMemberId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var tenantmember = await ValidateTenantAccessAsync("task:create");

            var project = task.BoardColumn.Board.Sprint.Project;
            var projectMember = project.Members.FirstOrDefault(c => c.TenantMemberId == tenantmember.Id);
            if (projectMember == null) throw new ProjectNotFoundException("Proejct member not found");

            task.AssignUser(projectMember);
            await _taskRepository.UpdateAsync(task);
        }

        public async Task UnAssignsMemberAsync(Guid taskId, Guid projectMemberId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var member = task.AssigneeMembers.FirstOrDefault(m => m.Id == projectMemberId);
            if (member == null) throw new KeyNotFoundException("Member not found");

            task.UnassignUser(member);
            await _taskRepository.UpdateAsync(task);
        }
    }
}