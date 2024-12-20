using AutoMapper;
using Bonyan.Layer.Domain.Enumerations;
using Bonyan.Layer.Domain.Model;
using Bonyan.MultiTenant;
using Bonyan.TenantManagement.Domain;
using PMS.Application.UseCases.Projects.Exceptions;
using PMS.Application.UseCases.Projects.Models;
using PMS.Application.UseCases.Projects.Specs;
using PMS.Application.UseCases.Tenants.Exceptions;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.UseCases.Projects
{
    public class ProjectService :  IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentTenant _currentTenant;

        public ProjectService(
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            IBoardRepository boardRepository,
            ITenantMemberRepository tenantMemberRepository,
            IProjectMemberRepository projectMemberRepository,
            IMapper mapper,
            ICurrentTenant  currentTenant
           )
        {
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _boardRepository = boardRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
            _currentTenant = currentTenant;
        }

        // Get projects for the current tenantEntity with optional filtering
        public async Task<PaginatedResult<ProjectDto>> GetProjectsAsync(ProjectFilterDto filter)
        {
            // Ensure the tenantEntity is validated (both exists and user is a part of it)

            // Fetching projects based on tenantEntity ID
            var projects = await _projectRepository.PaginatedAsync(new ProjectsByTenantSpec(_currentTenant.Id.Value, filter));
            return _mapper.Map<PaginatedResult<ProjectDto>>(projects);
        }

        // Get detailed information about a project
        public async Task<ProjectDto> GetProjectDetailsAsync(Guid projectId)
        {
            // Ensure the tenantEntity is validated (both exists and user is a part of it)

            // Fetch project by ID for the current tenantEntity
            var project =
                await _projectRepository.FindOneAsync(
                    new ProjectDetailsByIdForTenantSpec( _currentTenant.Id.Value,projectId));
            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            return _mapper.Map<ProjectDto>(project);
        }

        // Create a new project in the current tenantEntity
        public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectCreateDto,Guid id)
        {
            // Ensure the tenantEntity is validated and the user has permission to create a project

            // Check if the current user has permission to remove members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(id, TenantId.FromGuid(_currentTenant.Id.Value));
            if (tenantMember == null )
            {
                throw new UnauthorizedAccessException("not found.");
            }
            
            // Create a new project entity
            var project = new ProjectEntity(projectCreateDto.Name, projectCreateDto.Description, projectCreateDto.StartDate);
            project.AddMember(new ProjectMemberEntity(tenantMember,project,ProjectMemberAccessEnum.ProductOwner));
            await _projectRepository.AddAsync(project);
            
            
            // Create a default sprint for the project
            var defaultSprint = new ProjectSprintEntity(project,"Default Sprint", projectCreateDto.StartDate,
                projectCreateDto.StartDate.AddMonths(1));
            await _sprintRepository.AddAsync(defaultSprint);

            // Create a default Kanban board with columns (ToDo, Doing, Done)
            var board = new KanbanBoardEntity("Default Kanban Board", defaultSprint);
            await _boardRepository.AddAsync(board);

            return _mapper.Map<ProjectDto>(project);
        }

        // Update an existing project
        public async Task<ProjectDto?> UpdateProjectAsync(Guid projectId, ProjectUpdateDto projectUpdateDto)  
        {
            // Ensure the tenantEntity is validated and the user has permission to update the project

            // Fetch the project by ID
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }

            // Update the project details
            project.UpdateDetails(projectUpdateDto.Name, projectUpdateDto.Description, projectUpdateDto.EndDate);
            await _projectRepository.UpdateAsync(project);

            return await GetProjectDetailsAsync(projectId);
        }

        // Delete a project
        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            // Ensure the tenantEntity is validated and the user has permission to delete the project

            // Fetch the project by ID
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }

            // Delete the project
            await _projectRepository.DeleteAsync(project);
            return true;
        }

        // Add a member to a project
        public async Task<ProjectMemberDto> AddMemberAsync(Guid projectId,  ProjectAddMemberDto projectAddMemberDto)
        {

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }

            // Retrieve the tenantEntity member by ID to ensure they exist
            var tenantMember = await _tenantMemberRepository.GetByIdAsync(projectAddMemberDto.TenantMemberId);
            if (tenantMember == null)
            {
                throw new TenantNotFoundException("Tenants Member not Found");
            }
            
            var member = new ProjectMemberEntity( tenantMember, project, Enumeration.FromName<ProjectMemberAccessEnum>(projectAddMemberDto.Role));
            project.AddMember(member);
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectMemberDto>(member);
        }

        // Remove a member from a project
        public async Task<bool> RemoveMemberAsync(Guid projectId, Guid memberId)
        {

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }

            var member = project.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                throw new MemberNotFoundException("can not found member");
            }

            project.RemoveMember(member);
            await _projectRepository.UpdateAsync(project);

            return true;
        }

        // Get members of a project with optional filtering
        public async Task<PaginatedResult<ProjectMemberDto>> GetMembersAsync(Guid projectId,
            ProjectMemberFilterDto filter)
        {

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }
            
            var paginatedResult =
                await _projectMemberRepository.PaginatedAsync(new ProjectMembersByProjectSpec(project.Id, filter));
            
            return _mapper.Map<PaginatedResult<ProjectMemberDto>>(paginatedResult);
        }

        // Update member details
        public async Task<ProjectMemberDto> UpdateMemberAsync(Guid projectId, Guid memberId,
            ProjectUpdateMemberDto projectUpdateMemberDto)
        {

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != _currentTenant.Id.Value)
            {
                throw new ProjectNotFoundException();
            }

            var member = project.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                throw new MemberNotFoundException("cannot be found member");
            }

            project.UpdateMemberAccess(memberId,Enumeration.FromName<ProjectMemberAccessEnum>(projectUpdateMemberDto.Role));
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectMemberDto>(member);
        }
    }
}