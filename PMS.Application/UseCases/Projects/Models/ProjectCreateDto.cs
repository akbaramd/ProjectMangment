namespace PMS.Application.UseCases.Projects.Models;

public class ProjectCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
}