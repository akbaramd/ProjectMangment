namespace PMS.Application.UseCases.Sprints.Models;

public class SprintUpdateDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}