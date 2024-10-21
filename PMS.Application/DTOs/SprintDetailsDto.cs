namespace PMS.Application.DTOs;

internal class SprintDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<TaskDto> Tasks { get; set; }
    public List<BoardDto> Boards { get; set; }
}