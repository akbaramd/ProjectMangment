using Bonyan.Layer.Domain.Model;
using PMS.Application.UseCases.Projects.Models;

namespace PMS.Application.UseCases.Projects
{
    public interface IProjectService
    {
        // Project CRUD operations
        Task<PaginatedResult<ProjectDto>> GetProjectsAsync(ProjectFilterDto filter);
        Task<ProjectDto> GetProjectDetailsAsync(Guid projectId);
        Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectCreateDto,Guid userId);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<ProjectDto?> UpdateProjectAsync(Guid projectId, ProjectUpdateDto projectUpdateDto);

        // Project member-related operations
        Task<ProjectMemberDto> AddMemberAsync(Guid projectId, ProjectAddMemberDto projectAddMemberDto);
        Task<bool> RemoveMemberAsync(Guid projectId, Guid memberId);
        Task<PaginatedResult<ProjectMemberDto>> GetMembersAsync(Guid projectId, ProjectMemberFilterDto filter);
        Task<ProjectMemberDto> UpdateMemberAsync(Guid projectId, Guid memberId, ProjectUpdateMemberDto projectUpdateMemberDto);
    }
}