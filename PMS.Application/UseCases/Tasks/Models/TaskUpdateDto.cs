namespace PMS.Application.UseCases.Tasks.Models;

public class TaskUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public int? Order { get; set; }
    public Guid? BoardColumnId { get; set; }
    public DateTime? DueDate { get; set; }
}