using System.Linq.Dynamic.Core;
using AutoMapper;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Domain.BoundedContexts.TaskManagment;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Tasks;

public class TaskProfile : Profile{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>().ReverseMap();
        CreateMap<TaskCommentEntity, TaskCommentDto>().ReverseMap();
        CreateMap<PaginatedResult<TaskEntity>,PaginatedResult<TaskDto>>().ReverseMap();
    }
}