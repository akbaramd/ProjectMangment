namespace PMS.Application.UseCases.Boards.Models;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}

public class BoardColumnUpdateDto
{
    
    public string Name { get; set; }
    public int Order { get; set; }
}