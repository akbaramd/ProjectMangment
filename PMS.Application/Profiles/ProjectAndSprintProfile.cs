using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Model;

namespace PMS.Application.Profiles
{
    public class ProjectAndSprintProfile : Profile
    {
        public ProjectAndSprintProfile()
        {
            // Project and Sprint Mappings
            CreateMap<Project, ProjectDetailDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Sprint, SprintDetailsDto>().ReverseMap();
            CreateMap<Sprint, SprintDto>().ReverseMap();
            CreateMap<Board, BoardDto>().ReverseMap();
            CreateMap<BoardColumn, BoardColumnDto>().ReverseMap();
            CreateMap<SprintTask, TaskDto>().ReverseMap();

            // Paginated Result Mappings
            CreateMap<PaginatedResult<Project>, PaginatedResult<ProjectDto>>().ReverseMap();
            CreateMap<PaginatedResult<Sprint>, PaginatedResult<SprintDto>>().ReverseMap();
            CreateMap<PaginatedResult<Board>, PaginatedResult<BoardDto>>().ReverseMap();
        }
    }
}