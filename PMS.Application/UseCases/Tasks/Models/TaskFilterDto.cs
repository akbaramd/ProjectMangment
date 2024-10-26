namespace PMS.Application.UseCases.Tasks.Models;

public class TaskFilterDto
{
    public string? Search { get; set; }
    public  Guid BoardId { get; set; }
    public  Guid? ColumnId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}