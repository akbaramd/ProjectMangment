using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedKernel.Model;

namespace PMS.Application.Interfaces
{
    public interface IProjectService
    {
        // Project CRUD operations
        Task<PaginatedResult<ProjectDto>> GetProjectsAsync(ProjectFilterDto filter);
        Task<ProjectDto> GetProjectDetailsAsync(Guid projectId);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<ProjectDetailDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto updateProjectDto);

        // Project member-related operations
        Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto addMemberDto);
        Task<bool> RemoveMemberAsync(Guid projectId, Guid memberId);
        Task<PaginatedResult<ProjectMemberDto>> GetMembersAsync(Guid projectId, ProjectMemberFilterDto filter);
        Task<ProjectMemberDto> UpdateMemberAsync(Guid projectId, Guid memberId, UpdateProjectMemberDto updateMemberDto);
    }
}