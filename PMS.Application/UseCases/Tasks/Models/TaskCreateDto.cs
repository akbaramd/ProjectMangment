namespace PMS.Application.UseCases.Tasks.Models;

public class TaskCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public Guid BoardColumnId { get; set; }
    public Guid BoardId { get; set; }
}