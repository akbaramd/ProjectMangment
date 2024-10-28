using AutoMapper;
using Bonyan.DomainDrivenDesign.Domain.Model;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;

namespace PMS.Application.UseCases.Tasks;

public class TaskProfile : Profile{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>().ReverseMap();
        CreateMap<TaskCommentEntity, TaskCommentDto>().ReverseMap();
        CreateMap<PaginatedResult<TaskEntity>,PaginatedResult<TaskDto>>().ReverseMap();
    }
}