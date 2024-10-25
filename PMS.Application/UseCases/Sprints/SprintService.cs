using AutoMapper;
using PMS.Application.Base;
using PMS.Application.UseCases.Projects.Exceptions;
using PMS.Application.UseCases.Sprints.Exceptions;
using PMS.Application.UseCases.Sprints.Models;
using PMS.Application.UseCases.Sprints.Specs;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Repositories;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Sprints
{
    public class SprintService : BaseTenantService, ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            IMapper mapper,
            IServiceProvider serviceProvider, IProjectRepository projectRepository)
            : base(serviceProvider)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        // Get all sprints for a specific project
        public async Task<PaginatedResult<SprintDto>> GetSprintsAsync(SprintFilterDto dto)
        {
            // Validate that tenantEntity exists and user has permission to view project sprints
            await ValidateTenantAccessAsync("sprint:read");

            // Fetch sprints for the project
            var sprints = await _sprintRepository.PaginatedAsync(new SprintsByTenantSpec(CurrentTenant.Id, dto));
            return _mapper.Map<PaginatedResult<SprintDto>>(sprints);
        }

        // Get details of a specific sprint by ID
        public async Task<SprintDetailsDto> GetSprintDetailsAsync(Guid sprintId)
        {
            // Validate tenantEntity and user permissions
            await ValidateTenantAccessAsync("sprint:read");

            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                throw new SprintNotFoundException();
            }

            return _mapper.Map<SprintDetailsDto>(sprint);
        }

        // Create a new sprint for a project
        public async Task<SprintDto> CreateSprintAsync(SprintCreateDto sprintCreateDto)
        {
            // Validate tenantEntity and user permission to create sprints
            await ValidateTenantAccessAsync("sprint:create");

            var project = await _projectRepository.FindOneAsync(x=>x.Id == sprintCreateDto.ProjectId);
            
            if (project == null )
            {
                throw new ProjectNotFoundException("not found.");
            }
            
            // Create a new sprint entity
            var sprint = new ProjectSprintEntity(project,sprintCreateDto.Name, sprintCreateDto.StartDate, sprintCreateDto.EndDate,
                CurrentTenant);

            // Add the sprint to the repository
            await _sprintRepository.AddAsync(sprint);

            return _mapper.Map<SprintDto>(sprint);
        }

        // Update an existing sprint
        public async Task<SprintDto> UpdateSprintAsync(Guid sprintId, SprintUpdateDto sprintUpdateDto)
        {
            // Validate tenantEntity and user permission to update sprints
            await ValidateTenantAccessAsync("sprint:update");

            // Fetch the sprint from the repository
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                throw new SprintNotFoundException();
            }

            // Update the sprint details
            sprint.UpdateDetails(sprintUpdateDto.Name, sprintUpdateDto.StartDate, sprintUpdateDto.EndDate);

            await _sprintRepository.UpdateAsync(sprint);

            return _mapper.Map<SprintDetailsDto>(sprint);
        }

        // Delete a sprint
        public async Task<bool> DeleteSprintAsync(Guid sprintId)
        {
            // Validate tenantEntity and user permission to delete sprints
            await ValidateTenantAccessAsync("sprint:remove");

            // Fetch the sprint
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                throw new SprintNotFoundException();
            }

            // Delete the sprint
            await _sprintRepository.DeleteAsync(sprint);
            return true;
        }
    }
}