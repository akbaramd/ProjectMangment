using PMS.Application.UseCases.Sprints.Models;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Sprints
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