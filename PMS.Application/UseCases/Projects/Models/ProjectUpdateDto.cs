namespace PMS.Application.UseCases.Projects.Models;

public class ProjectUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EndDate { get; set; }
}