namespace PMS.Application.UseCases.Sprints.Models;

public class SprintDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}