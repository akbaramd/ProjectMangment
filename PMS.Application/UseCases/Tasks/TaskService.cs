using AutoMapper;
using Bonyan.DomainDrivenDesign.Domain.Events;
using PMS.Application.Base;
using PMS.Application.UseCases.Projects.Exceptions;
using PMS.Application.UseCases.Sprints.Exceptions;
using PMS.Application.UseCases.Tasks.Exceptions;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Application.UseCases.Tasks.Specs;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks.Repositories;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Tasks
{
    public class TaskService : BaseTenantService, ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public TaskService(
            ITaskRepository taskRepository,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IDomainEventDispatcher domainEventDispatcher, IBoardRepository boardRepository, IProjectRepository projectRepository) : base(serviceProvider)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _domainEventDispatcher = domainEventDispatcher;
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
        }

        public async Task<PaginatedResult<TaskDto>> GetTasksAsync(TaskFilterDto filter)
        {
            await ValidateTenantAccessAsync("task:read");
            var tasks = await _taskRepository.PaginatedAsync(new GetTaskBySprintSpec(filter));
            return _mapper.Map<PaginatedResult<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskDetailsAsync(Guid taskId)
        {
            await ValidateTenantAccessAsync("task:read");
            var task = await _taskRepository.FindOneAsync(new GetTaskDetailsByIdSpec(taskId));
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(TaskCreateDto taskCreateDto)
        {
            await ValidateTenantAccessAsync("task:create");

            var board = await _boardRepository.FindByIdAsync(taskCreateDto.BoardId);
            if (board == null) throw new ColumnNotFoundException($"Board with ID {taskCreateDto.BoardId} not found");

            var column = board.Columns.FirstOrDefault(x => x.Id == taskCreateDto.BoardColumnId);
            if (column == null) throw new ColumnNotFoundException($"BoardColumn with ID {taskCreateDto.BoardColumnId} not found");
            
            var order = (await _taskRepository.FindAsync(x=>x.BoardColumnId == column.Id)).MaxBy(x => x.Order)?.Order ?? 0;
            var taskEntity = new TaskEntity(taskCreateDto.Title, taskCreateDto.Description,
                order + 1, column, CurrentTenant);

            await _taskRepository.AddAsync(taskEntity);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { taskEntity });

            return await GetTaskDetailsAsync(taskEntity.Id);
        }

        public async Task<TaskDto?> UpdateTaskAsync(Guid taskId, TaskUpdateDto taskUpdateDto)
        {
            
            
            
            await ValidateTenantAccessAsync("task:update");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            if (taskUpdateDto.Title != null)
                task.UpdateTitle(taskUpdateDto.Title);
            
            if (taskUpdateDto.Description != null)
                task.UpdateDescription(taskUpdateDto.Description);
            
            
            if (taskUpdateDto.DueDate.HasValue)
                task.UpdateDueDate(taskUpdateDto.DueDate.Value);

            // اضافه کردن به‌روزرسانی ترتیب
            if (taskUpdateDto.Order.HasValue)
                task.UpdateOrder(taskUpdateDto.Order.Value);

            // اضافه کردن به‌روزرسانی ستون
            if (taskUpdateDto is { BoardColumnId: not null, BoardId: not null })
            {
                var board = await _boardRepository.FindByIdAsync(taskUpdateDto.BoardId.Value);
                if (board == null) throw new ColumnNotFoundException($"Board with ID {taskUpdateDto.BoardId} not found");
                
                var column = board.Columns.FirstOrDefault(x => x.Id == taskUpdateDto.BoardColumnId);
                if (column == null) throw new ColumnNotFoundException($"BoardColumn with ID {taskUpdateDto.BoardColumnId} not found");
                
                task.MoveToColumn(column);
            }
            
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            await ValidateTenantAccessAsync("task:remove");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });

            return true;
        }

        public async Task AddCommentAsync(Guid taskId, TaskCommentCreateDto createDto)
        {
            await ValidateTenantAccessAsync("task:comment");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            var tenantMember = await ValidateTenantAccessAsync("task:create");
            var projectMember = await _projectRepository.GetMemberByTenantMemberIdAsync(tenantMember.Id);
            if (projectMember == null) throw new ProjectMemberNotFoundException("Project member not found");

            task.AddComment(projectMember, createDto.Content);
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });
        }

        public async Task EditCommentAsync(Guid taskId, Guid commentId, TaskCommentUpdateDto updateDto)
        {
            await ValidateTenantAccessAsync("task:comment");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) throw new TaskCommentNotFoundException($"Comment with ID {commentId} not found");

            comment.UpdateContent(updateDto.Content);
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });
        }

        public async Task DeleteCommentAsync(Guid taskId, Guid commentId)
        {
            await ValidateTenantAccessAsync("task:comment");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) throw new TaskCommentNotFoundException($"Comment with ID {commentId} not found");

            task.DeleteComment(commentId);
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });
        }

        public async Task AssignMemberAsync(Guid taskId, Guid projectMemberId)
        {
            await ValidateTenantAccessAsync("task:assign");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            var tenantMember = await ValidateTenantAccessAsync("task:assign");
            var projectMember = await _projectRepository.GetMemberByIdAsync(projectMemberId);

            if (projectMember == null) throw new ProjectMemberNotFoundException($"Project member not found");

            task.AssignUser(projectMember);
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });
        }

        public async Task UnassignMemberAsync(Guid taskId, Guid projectMemberId)
        {
            await ValidateTenantAccessAsync("task:unassign");

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new TaskNotFoundException($"Task with ID {taskId} not found");

            var member = task.AssigneeMembers.FirstOrDefault(m => m.Id == projectMemberId);
            if (member == null) throw new ProjectMemberNotFoundException($"Member with ID {projectMemberId} not found");

            task.UnassignUser(member);
            await _taskRepository.UpdateAsync(task);
            await _domainEventDispatcher.DispatchAndClearEvents(new[] { task });
        }
    }
}
