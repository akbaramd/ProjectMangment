using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PMS.Application.UseCases.Projects.Specs;
using SharedKernel.Model;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.Services
{
    public class ProjectService : BaseTenantService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            IBoardRepository boardRepository,
            IBoardColumnRepository boardColumnRepository,
            ITenantMemberRepository tenantMemberRepository,
            IProjectMemberRepository projectMemberRepository,
            IMapper mapper,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _boardRepository = boardRepository;
            _boardColumnRepository = boardColumnRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        // Get projects for the current tenant with optional filtering
        public async Task<PaginatedResult<ProjectDto>> GetProjectsAsync(ProjectFilterDto filter)
        {
            // Ensure the tenant is validated (both exists and user is a part of it)
            await ValidateTenantAccessAsync("project:read");

            // Fetching projects based on tenant ID
            var projects = await _projectRepository.PaginatedAsync(new ProjectsByTenantSpec(CurrentTenant.Id, filter));
            return _mapper.Map<PaginatedResult<ProjectDto>>(projects);
        }

        // Get detailed information about a project
        public async Task<ProjectDto> GetProjectDetailsAsync(Guid projectId)
        {
            // Ensure the tenant is validated (both exists and user is a part of it)
            await ValidateTenantAccessAsync("project:read");

            // Fetch project by ID for the current tenant
            var project =
                await _projectRepository.FindOneAsync(
                    new ProjectDetailsByIdForTenantSpec(projectId, CurrentTenant.Id));
            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            return _mapper.Map<ProjectDto>(project);
        }

        // Create a new project in the current tenant
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            // Ensure the tenant is validated and the user has permission to create a project
            await ValidateTenantAccessAsync("project:create");

            // Check if the current user has permission to remove members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(CurrentUser.Id, CurrentTenant.Id);
            if (tenantMember == null )
            {
                throw new UnauthorizedAccessException("not found.");
            }
            
            // Create a new project entity
            var project = new Project(createProjectDto.Name, createProjectDto.Description, createProjectDto.StartDate,
                CurrentTenant);
            project.AddMember(new ProjectMember(CurrentTenant,tenantMember,project,ProjectMemberAccess.ProductOwner.Name));
            await _projectRepository.AddAsync(project);
            
            
            // Create a default sprint for the project
            var defaultSprint = new Sprint("Default Sprint", createProjectDto.StartDate,
                createProjectDto.StartDate.AddMonths(1), CurrentTenant);
            project.AddSprint(defaultSprint);
            await _sprintRepository.AddAsync(defaultSprint);

            // Create a default Kanban board with columns (ToDo, Doing, Done)
            var board = new Board("Default Kanban Board", defaultSprint, CurrentTenant);
            await _boardRepository.AddAsync(board);

            return _mapper.Map<ProjectDto>(project);
        }

        // Update an existing project
        public async Task<ProjectDetailDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto updateProjectDto)
        {
            // Ensure the tenant is validated and the user has permission to update the project
            await ValidateTenantAccessAsync("project:update");

            // Fetch the project by ID
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
            {
                throw new ProjectNotFoundException();
            }

            // Update the project details
            project.UpdateDetails(updateProjectDto.Name, updateProjectDto.Description, updateProjectDto.EndDate);
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectDetailDto>(project);
        }

        // Delete a project
        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            // Ensure the tenant is validated and the user has permission to delete the project
            await ValidateTenantAccessAsync("project:delete");

            // Fetch the project by ID
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
            {
                throw new ProjectNotFoundException();
            }

            // Delete the project
            await _projectRepository.DeleteAsync(project);
            return true;
        }

        // Add a member to a project
        public async Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto addMemberDto)
        {
            await ValidateTenantAccessAsync("project:member:add");

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
            {
                throw new ProjectNotFoundException();
            }

            // Retrieve the tenant member by ID to ensure they exist
            var tenantMember = await _tenantMemberRepository.GetByIdAsync(addMemberDto.TenantMemberId);
            if (tenantMember == null)
            {
                throw new TenantNotFoundException("Tenant Member not Found");
            }

            var member = new ProjectMember(CurrentTenant, tenantMember, project, addMemberDto.Role);
            project.AddMember(member);
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectMemberDto>(member);
        }

        // Remove a member from a project
        public async Task<bool> RemoveMemberAsync(Guid projectId, Guid memberId)
        {
            await ValidateTenantAccessAsync("project:member:remove");

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
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
            await ValidateTenantAccessAsync("project:member:read");

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
            {
                throw new ProjectNotFoundException();
            }


            var paginatedResult =
                await _projectMemberRepository.PaginatedAsync(new ProjectMembersByProjectSpec(project.Id, filter));
            return _mapper.Map<PaginatedResult<ProjectMemberDto>>(paginatedResult);
        }

        // Update member details
        public async Task<ProjectMemberDto> UpdateMemberAsync(Guid projectId, Guid memberId,
            UpdateProjectMemberDto updateMemberDto)
        {
            await ValidateTenantAccessAsync("project:member:update");

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != CurrentTenant.Id)
            {
                throw new ProjectNotFoundException();
            }

            var member = project.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                throw new MemberNotFoundException("cannot be found member");
            }

            member.UpdateDetails(updateMemberDto.Role);
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectMemberDto>(member);
        }
    }
}