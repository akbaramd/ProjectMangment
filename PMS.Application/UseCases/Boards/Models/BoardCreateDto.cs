namespace PMS.Application.UseCases.Boards.Models;

public class BoardCreateDto
{
    public Guid SprintId { get; set; }
    public string Name { get; set; }
}