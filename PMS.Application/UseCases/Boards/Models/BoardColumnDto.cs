namespace PMS.Application.UseCases.Boards.Models;

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}