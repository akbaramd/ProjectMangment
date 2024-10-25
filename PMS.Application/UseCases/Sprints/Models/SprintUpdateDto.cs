namespace PMS.Application.UseCases.Sprints.Model;

public class SprintUpdateDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}