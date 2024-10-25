using PMS.Application.UseCases.Projects.Models;

namespace PMS.Application.UseCases.Sprints.Models;

public class SprintDetailsDto : SprintDto
{
    public ProjectDto Project { get; set; }
}