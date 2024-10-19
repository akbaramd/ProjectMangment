using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            IBoardRepository boardRepository,
            IBoardColumnRepository boardColumnRepository,
            ITenantRepository tenantRepository,
            ITenantMemberRepository tenantMemberRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _boardRepository = boardRepository;
            _boardColumnRepository = boardColumnRepository;
            _tenantRepository = tenantRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _mapper = mapper;
        }

        // Create a new project, with default sprint and Kanban board setup

        // Get project list for a tenant
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto,string tenantName, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantName);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Validate if the user has permission to create a project
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:create"))
            {
                throw new UnauthorizedAccessException("You do not have permission to create projects.");
            }

            // Create the project
            var project = new Project(createProjectDto.Name, createProjectDto.Description, createProjectDto.StartDate, tenant);
            await _projectRepository.AddAsync(project);

            // Create a default sprint
            var defaultSprint = new Sprint("Default Sprint", createProjectDto.StartDate, createProjectDto.StartDate.AddMonths(1), tenant);
            project.AddSprint(defaultSprint);
            await _sprintRepository.AddAsync(defaultSprint);

            // Create a default board with columns (ToDo, Doing, Done)
            var board = new Board("Default Kanban Board",defaultSprint, tenant);

            await _boardRepository.AddAsync(board);

            var projectDto = _mapper.Map<ProjectDto>(project);
            return projectDto;
        }

        public async Task<List<ProjectDto>> GetProjectListAsync(string tenantSubdomain)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var projects = _projectRepository.GetByTenantId(tenant.Id);
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        // Get detailed information about a project
        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, string tenantSubdomain)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var project = await _projectRepository.GetByIdWithRelationsAsync(projectId);
            if (project == null || project.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            return new ProjectDetailsDto()
            {
                Project = _mapper.Map<ProjectDto>(project),
                Sprints = _mapper.Map<List<SprintDto>>(project.Sprints),
                Boards = _mapper.Map<List<BoardDto>>(project.Sprints.SelectMany(x=>x.Boards)),
            };
        }

        // Update an existing project
        public async Task<ProjectDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto updateProjectDto, string tenantSubdomain, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Validate if the user has permission to update the project
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:update"))
            {
                throw new UnauthorizedAccessException("You do not have permission to update projects.");
            }

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            project.UpdateDetails(updateProjectDto.Name, updateProjectDto.Description, updateProjectDto.EndDate);
            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectDto>(project);
        }

        // Delete a project
        public async Task<bool> DeleteProjectAsync(Guid projectId, string tenantSubdomain, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Validate if the user has permission to delete the project
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:delete"))
            {
                throw new UnauthorizedAccessException("You do not have permission to delete projects.");
            }

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null || project.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}
