namespace PMS.Application.DTOs;

public class ProjectFilterDto
{
    public ProjectFilterDto(int take, int skip, string? search)
    {
        Take = take;
        Skip = skip;
        Search = search;
    }

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    
    
    
}

public class CreateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
}

public class UpdateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid TenantId { get; set; }
}

public class ProjectDetailDto : ProjectDto
{
    public List<SprintDetailsDto> Sprints { get; set; }
}

