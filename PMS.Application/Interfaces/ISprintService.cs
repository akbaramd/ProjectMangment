using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PMS.Application.UseCases.Sprints.Model;
using SharedKernel.Model;

namespace PMS.Application.Interfaces
{
    public interface ISprintService
    {
        // Get all sprints by project ID
        Task<PaginatedResult<SprintDto>> GetSprintsAsync(SprintFilterDto dto);

        // Get details of a specific sprint
        Task<SprintDetailsDto> GetSprintDetailsAsync(Guid sprintId);

        // Create a new sprint
        Task<SprintDto> CreateSprintAsync(SprintCreateDto sprintCreateDto);

        // Update an existing sprint
        Task<SprintDto> UpdateSprintAsync(Guid sprintId, SprintUpdateDto sprintUpdateDto);

        // Delete a sprint
        Task<bool> DeleteSprintAsync(Guid sprintId);
    }
}