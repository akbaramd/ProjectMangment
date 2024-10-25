using AutoMapper;
using PMS.Application.UseCases.Boards.Models;
using PMS.Application.UseCases.Projects.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagment;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Projects
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
            

            // Paginated Result Mappings
            CreateMap<PaginatedResult<ProjectEntity>, PaginatedResult<ProjectDto>>().ReverseMap();

            CreateMap<PaginatedResult<ProjectBoardEntity>, PaginatedResult<BoardDto>>().ReverseMap();
            CreateMap<PaginatedResult<ProjectMemberEntity>, PaginatedResult<ProjectMemberDto>>().ReverseMap();
        }
    }
}