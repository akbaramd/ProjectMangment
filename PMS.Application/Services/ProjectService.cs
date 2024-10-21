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

namespace PMS.Application.Services
{
    public class ProjectService : BaseTenantService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            IBoardRepository boardRepository,
            IBoardColumnRepository boardColumnRepository,
            ITenantMemberRepository tenantMemberRepository,
            IMapper mapper,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _boardRepository = boardRepository;
            _boardColumnRepository = boardColumnRepository;
            _tenantMemberRepository = tenantMemberRepository;
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

            // Create a new project entity
            var project = new Project(createProjectDto.Name, createProjectDto.Description, createProjectDto.StartDate,
                CurrentTenant);
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
    }
}