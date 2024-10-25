namespace PMS.Application.UseCases.Projects.Models;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid TenantId { get; set; }
    public List<ProjectMemberDto> Members { get; set; }
}

public class ProjectMemberFilterDto
{

    public ProjectMemberFilterDto(int take, int skip, string? search)
    {
        Take = take;
        Skip = skip;
        Search = search;
    }

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    
    
    
}