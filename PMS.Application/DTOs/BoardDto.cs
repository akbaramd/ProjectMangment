namespace PMS.Application.DTOs;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}