using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.UseCases.Sprints.Model;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagment;
using SharedKernel.Model;

namespace PMS.Application.Profiles
{
    public class ProjectAndSprintProfile : Profile
    {
        public ProjectAndSprintProfile()
        {
            // Project and Sprint Mappings
            CreateMap<ProjectEntity, ProjectDetailDto>().ReverseMap();
            CreateMap<ProjectMemberEntity, ProjectMemberDto>().ReverseMap();
            CreateMap<ProjectEntity, ProjectDto>().ReverseMap();
           
            CreateMap<ProjectBoardEntity, BoardDto>().ReverseMap();
            CreateMap<ProjectBoardColumnEntity, BoardColumnDto>().ReverseMap();
            CreateMap<TaskEntity, TaskDto>().ReverseMap();

            // Paginated Result Mappings
            CreateMap<PaginatedResult<ProjectEntity>, PaginatedResult<ProjectDto>>().ReverseMap();

            CreateMap<PaginatedResult<ProjectBoardEntity>, PaginatedResult<BoardDto>>().ReverseMap();
            CreateMap<PaginatedResult<ProjectMemberEntity>, PaginatedResult<ProjectMemberDto>>().ReverseMap();
        }
    }
}