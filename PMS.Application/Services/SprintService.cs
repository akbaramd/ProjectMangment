using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.UseCases.Sprints.Specs;
using SharedKernel.Model;

namespace PMS.Application.Services
{
    public class SprintService : BaseTenantService, ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            IMapper mapper,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        // Get all sprints for a specific project
        public async Task<PaginatedResult<SprintDto>> GetSprintsAsync(SprintFilterDto dto)
        {
            // Validate that tenant exists and user has permission to view project sprints
            await ValidateTenantAccessAsync("sprint:read");

            // Fetch sprints for the project
            var sprints = await _sprintRepository.PaginatedAsync(new SprintsByTenantSpec(CurrentTenant.Id, dto));
            return _mapper.Map<PaginatedResult<SprintDto>>(sprints);
        }

        // Get details of a specific sprint by ID
        public async Task<SprintDetailsDto> GetSprintDetailsAsync(Guid sprintId)
        {
            // Validate tenant and user permissions
            await ValidateTenantAccessAsync("sprint:read");

            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                throw new SprintNotFoundException();
            }

            return _mapper.Map<SprintDetailsDto>(sprint);
        }

        // Create a new sprint for a project
        public async Task<SprintDto> CreateSprintAsync(CreateSprintDto createSprintDto)
        {
            // Validate tenant and user permission to create sprints
            await ValidateTenantAccessAsync("sprint:create");

            // Create a new sprint entity
            var sprint = new Sprint(createSprintDto.Name, createSprintDto.StartDate, createSprintDto.EndDate,
                CurrentTenant);

            // Add the sprint to the repository
            await _sprintRepository.AddAsync(sprint);

            return _mapper.Map<SprintDto>(sprint);
        }

        // Update an existing sprint
        public async Task<SprintDto> UpdateSprintAsync(Guid sprintId, UpdateSprintDto updateSprintDto)
        {
            // Validate tenant and user permission to update sprints
            await ValidateTenantAccessAsync("sprint:update");

            // Fetch the sprint from the repository
            var sprint = await _sprintRepository.GetByIdAsync(sprintId);
            if (sprint == null)
            {
                throw new SprintNotFoundException();
            }

            // Update the sprint details
            sprint.UpdateDetails(updateSprintDto.Name, updateSprintDto.StartDate, updateSprintDto.EndDate);

            await _sprintRepository.UpdateAsync(sprint);

            return _mapper.Map<SprintDetailsDto>(sprint);
        }

        // Delete a sprint
        public async Task<bool> DeleteSprintAsync(Guid sprintId)
        {
            // Validate tenant and user permission to delete sprints
            await ValidateTenantAccessAsync("sprint:delete");

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