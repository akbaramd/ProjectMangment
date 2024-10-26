using PMS.Application.UseCases.Projects.Models;
using PMS.Application.UseCases.Tasks.Models;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagment;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Tasks.Specs;

public class GetTaskBySprintSpec : PaginatedSpecification<TaskEntity>
{
  
    public  TaskFilterDto Filter{ get; set; }

    public GetTaskBySprintSpec(TaskFilterDto dto) : base(dto.Skip,dto.Take)
    {
        Filter = dto;
    }

    public override void Handle(ISpecificationContext<TaskEntity> context)
    {
        context.AddInclude(x => x.BoardColumn).ThenInclude(x => x.Board)
            .ThenInclude(x=>x.Sprint);
        context. AddCriteria(x => x.BoardColumn.Board.SprintId == Filter.SprintId);


        if (Filter.BoardId != null)
        {
            context.AddCriteria(x=>x.BoardColumn.BoardId == Filter.BoardId);
        }
        
        if (Filter.ColumnId != null)
        {
            context.AddCriteria(x=>x.BoardColumnId == Filter.ColumnId);
        }
        
        if (Filter.Search != null && !string.IsNullOrWhiteSpace(Filter.Search))
        {
            context.AddCriteria(c => c.Title.Contains(Filter.Search) || c.Content.Contains(Filter.Search) );
        }
    }
}