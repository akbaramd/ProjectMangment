using AutoMapper;
using PMS.Application.UseCases.Sprints.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using SharedKernel.Model;

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