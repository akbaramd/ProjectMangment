using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto,string tenant, Guid userId);
        Task<List<ProjectDto>> GetProjectListAsync(string tenantId);
        Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, string tenantId);
        Task<bool> DeleteProjectAsync(Guid projectId, string tenantId, Guid userId);
        Task<ProjectDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto updateProjectDto, string tenantId, Guid userId);

    }
}