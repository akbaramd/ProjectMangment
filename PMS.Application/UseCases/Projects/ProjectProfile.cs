using AutoMapper;
using PMS.Application.UseCases.Boards.Models;
using PMS.Application.UseCases.Projects.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;
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
           
            CreateMap<KanbanBoardEntity, BoardDto>().ReverseMap();
            CreateMap<KanbanBoardColumnEntity, BoardColumnDto>().ReverseMap();
            

            // Paginated Result Mappings
            CreateMap<PaginatedResult<ProjectEntity>, PaginatedResult<ProjectDto>>().ReverseMap();

            CreateMap<PaginatedResult<KanbanBoardEntity>, PaginatedResult<BoardDto>>().ReverseMap();
            CreateMap<PaginatedResult<ProjectMemberEntity>, PaginatedResult<ProjectMemberDto>>().ReverseMap();
        }
    }
}