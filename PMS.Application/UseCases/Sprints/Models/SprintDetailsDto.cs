using PMS.Application.DTOs;

namespace PMS.Application.UseCases.Sprints.Model;

public class SprintDetailsDto : SprintDto
{
    public ProjectDto Project { get; set; }
}