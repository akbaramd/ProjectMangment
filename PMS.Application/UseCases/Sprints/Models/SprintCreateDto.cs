namespace PMS.Application.UseCases.Sprints.Models;

public class SprintCreateDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProjectId { get; set; }
}