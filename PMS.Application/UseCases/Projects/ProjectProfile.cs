using AutoMapper;
using Bonyan.DomainDrivenDesign.Domain.Model;
using PMS.Application.UseCases.Projects.Models;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;

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
           
            // CreateMap<KanbanBoardEntity, BoardDto>().ReverseMap();
            // CreateMap<KanbanBoardColumnEntity, BoardColumnDto>().ReverseMap();
            

            // Paginated Result Mappings
            CreateMap<PaginatedResult<ProjectEntity>, PaginatedResult<ProjectDto>>().ReverseMap();

            // CreateMap<PaginatedResult<KanbanBoardEntity>, PaginatedResult<BoardDto>>().ReverseMap();
            CreateMap<PaginatedResult<ProjectMemberEntity>, PaginatedResult<ProjectMemberDto>>().ReverseMap();
        }
    }
}