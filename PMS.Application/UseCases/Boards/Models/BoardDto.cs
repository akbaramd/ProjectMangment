using PMS.Application.UseCases.Sprints.Models;

namespace PMS.Application.UseCases.Boards.Models;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}