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