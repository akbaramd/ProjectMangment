using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedKernel.Model;

namespace PMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<PaginatedResult<ProjectDto>> GetProjectsAsync( ProjectFilterDto filter);
        Task<ProjectDto> GetProjectDetailsAsync(Guid projectId);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<ProjectDetailDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto updateProjectDto);

    }
}