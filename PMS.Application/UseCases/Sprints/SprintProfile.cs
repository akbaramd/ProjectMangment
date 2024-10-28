using AutoMapper;
using Bonyan.DomainDrivenDesign.Domain.Model;
using PMS.Application.UseCases.Sprints.Models;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;

namespace PMS.Application.UseCases.Sprints;

public class SprintProfile : Profile
{
    public SprintProfile()
    {
        CreateMap<ProjectSprintEntity, SprintDetailsDto>().ReverseMap();
        CreateMap<ProjectSprintEntity, SprintDto>().ReverseMap();
        
        CreateMap<PaginatedResult<ProjectSprintEntity>, PaginatedResult<SprintDto>>().ReverseMap();
    }
}